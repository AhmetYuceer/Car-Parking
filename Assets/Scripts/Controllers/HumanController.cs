using System.Collections;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    [SerializeField] private Transform _firstPoint, _secondPoint;
    [SerializeField] private float _speed;
    [SerializeField] private float _idleDelay;

    private Animator _animator;
    private Transform _targetPoint;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _targetPoint = _firstPoint;
        StartCoroutine(MoveBetweenPoints());
    }

    private IEnumerator MoveBetweenPoints()
    {
        while (true)
        {
            _animator.SetBool("isMoving", true);

            while (Vector3.Distance(transform.position, _targetPoint.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPoint.position, _speed * Time.deltaTime);
                transform.LookAt(_targetPoint);
                yield return null;
            }
            _animator.SetBool("isMoving", false);
            yield return new WaitForSeconds(_idleDelay);
            _targetPoint = _targetPoint == _firstPoint ? _secondPoint : _firstPoint;
        }
    }
}