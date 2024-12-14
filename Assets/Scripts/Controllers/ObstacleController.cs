using System.Collections;
using UnityEngine;
using Interface;
using System.Collections.Generic;

public class ObstacleController : MonoBehaviour, IMoveable
{
    [SerializeField] private List<Vector3> _moveablePositions = new List<Vector3>();
    private bool _isMoving;
    private float _speed = 5f;
    private Vector3 _lastPosition;

    private void Start()
    {
        GenerateMoveablePositions();
        _lastPosition = transform.position;
    }

    public void Move(Direction direction)
    {
        if (_isMoving || _moveablePositions.Count == 0)
            return;

        Vector3 targetPosition = GetTargetPosition(direction);
 
        if (targetPosition != transform.position && targetPosition != _lastPosition)
        {
            GameManager.Instance.Move(1);        
            StartCoroutine(MoveToPosition(targetPosition));
            _lastPosition = targetPosition;
        } 
    }

    private void GenerateMoveablePositions()
    {
        Vector3 currentPosition = transform.position;
 
        _moveablePositions.Add(currentPosition);
        _moveablePositions.Add(currentPosition + Vector3.forward);
        _moveablePositions.Add(currentPosition - Vector3.forward);
        _moveablePositions.Add(currentPosition + Vector3.right);
        _moveablePositions.Add(currentPosition - Vector3.right);
    }

    private Vector3 GetTargetPosition(Direction direction)
    {
        Vector3 currentPos = transform.position;
        Vector3 targetPosition = currentPos;
        
        switch (direction)
        {
            case Direction.Forward:
                if (_moveablePositions.Contains(currentPos + Vector3.forward))
                    targetPosition = _moveablePositions.Find(pos => pos == currentPos + Vector3.forward);
                break;
            case Direction.Backward:
                if (_moveablePositions.Contains(currentPos - Vector3.forward))
                    targetPosition = _moveablePositions.Find(pos => pos == currentPos - Vector3.forward);
                break;
            case Direction.Right:
                if (_moveablePositions.Contains(currentPos + Vector3.right))
                    targetPosition = _moveablePositions.Find(pos => pos == currentPos + Vector3.right);
                break;
            case Direction.Left:
                if (_moveablePositions.Contains(currentPos - Vector3.right))
                    targetPosition = _moveablePositions.Find(pos => pos == currentPos - Vector3.right);
                break;
        }
         
        return targetPosition;
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        _isMoving = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        _isMoving = false;
    }
}
