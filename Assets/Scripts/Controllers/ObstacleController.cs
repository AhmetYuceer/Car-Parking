using System.Collections;
using UnityEngine;
using Interface;
using System.Collections.Generic;

public class ObstacleController : MonoBehaviour, IMoveable
{
    [SerializeField] private float _offset;
    [SerializeField] private List<Vector3> _moveablePositions = new List<Vector3>();
    [SerializeField]private bool _isMoving; 
    private readonly float _speed = 5f;
    [SerializeField] private Vector3 _lastPosition;
    
    private void Start()
    {
        //GenerateMoveablePositions();
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
    
    [ContextMenu("GenerateMoveablePositions")]
    private void GenerateMoveablePositions()
    {
        Vector3 currentPosition = transform.position;
 
        _moveablePositions.Add(currentPosition);
        _moveablePositions.Add(currentPosition +  Vector3.forward * _offset);
        _moveablePositions.Add(currentPosition -  Vector3.forward * _offset);
        _moveablePositions.Add(currentPosition +  Vector3.right * _offset);
        _moveablePositions.Add(currentPosition -  Vector3.right * _offset);
    }

    private Vector3 GetTargetPosition(Direction direction)
    {
        Vector3 currentPos = transform.position;
        Vector3 targetPosition = currentPos;
        Vector3 pos;

        switch (direction)
        {
            case Direction.Forward:
                pos = currentPos + Vector3.forward * _offset;
                if (_moveablePositions.Contains(pos))
                    targetPosition = pos;
                break;
            case Direction.Backward:
                 pos = currentPos - Vector3.forward * _offset;
                 if (_moveablePositions.Contains(pos))
                     targetPosition = pos;
                 break;
            case Direction.Right:
                 pos = currentPos + Vector3.right * _offset;
                 if (_moveablePositions.Contains(pos))
                    targetPosition = pos;
                 break;
            case Direction.Left:
                pos =  currentPos - Vector3.right * _offset;
                if (_moveablePositions.Contains(pos))
                    targetPosition =  pos;
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
