using Interface;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Camera _camera;
    private float _raycastDistance = 100f;
    private Vector2 _mouseDownPosition;
    private Vector2 _mouseUpPosition;
    private IMoveable _selectedObject;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.CheckMove())
            {
                _mouseDownPosition = Input.mousePosition;
                SelectObject();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (GameManager.Instance.CheckMove())
            {
                _mouseUpPosition = Input.mousePosition;
            
                if (_selectedObject != null)
                    DetermineDirection();
            }
        }
    }

    private void SelectObject()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance))
        {
            if (hit.collider.transform.TryGetComponent(out IMoveable moveableObject))
                _selectedObject = moveableObject;
            else
                _selectedObject = null;
        }
    }

    private void DetermineDirection()
    {
        Vector2 directionVector = _mouseUpPosition - _mouseDownPosition;

        if (directionVector.magnitude < 10f)
            return;

        Direction determinedDirection;

        if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.y))
        {
            if (directionVector.x > 0)
                determinedDirection = Direction.Right;
            else
                determinedDirection = Direction.Left;
        }
        else
        {
            if (directionVector.y > 0)
                determinedDirection = Direction.Forward;
            else
                determinedDirection = Direction.Backward;
        }
        
        MoveObject(determinedDirection);
    }

    private void MoveObject(Direction direction)
    {
        _selectedObject?.Move(direction);
    }
}