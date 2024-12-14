using System.Collections;
using UnityEngine;
using Interface;
using DG.Tweening;

public class CarController : MonoBehaviour, IMoveable
{

    [SerializeField] private Direction[] _unavailableDirections;
    [SerializeField] private bool _isMoving;
    [SerializeField] private ParticleSystem _smokeParticle;
    [SerializeField] private ParticleSystem _crashParticleEffect;
    private float _speed = 10f;
    private Rigidbody _rigidbody;
    private Vector3 _startingPosition;
    private Vector3 _startingRotation;
    private Vector3 _movementDirection;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _isMoving = false;
        _startingPosition = transform.position;
        _startingRotation = transform.rotation.eulerAngles;
        _movementDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            MoveForward();
        }
    }

    public void Move(Direction direction)
    {
        if (!_isMoving)
        {
            StartMoving(direction);
        }
    }

    private void MoveForward()
    {
        Vector3 step = _movementDirection * (_speed * Time.fixedDeltaTime);
        Vector3 newPosition = transform.position + step;
        _rigidbody.MovePosition(newPosition);
    }

    private void StartMoving(Direction direction)
    {
        foreach (var nothingDirection in _unavailableDirections)
        {
            if (nothingDirection == direction)
                return;
        }        
        
        _movementDirection = direction switch
        {
            Direction.Forward => Vector3.forward,
            Direction.Backward => Vector3.back,
            Direction.Right => Vector3.right,
            Direction.Left => Vector3.left,
            _ => Vector3.zero
        };

        if (_movementDirection == Vector3.zero)
        {
            return;
        }

        if (!_smokeParticle.isPlaying)
        {
            _smokeParticle.Play();
        }

        _isMoving = true;
    }

    private void StopMoving()
    {
        _smokeParticle.Stop();
        _isMoving = false;
        _movementDirection = Vector3.zero;
    }

    private IEnumerator Collision(Collision collision)
    {
        Crash(collision);
        yield return new WaitForSeconds(3f);
        RestartCar();
    }

    private void Crash(Collision collision)
    {
        CrashParticle(collision);
        DOTween.Kill("Rotate");
        StopMoving();
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void CrashParticle(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;

        if (contactPoints.Length != 0)
        {
            Vector3 middlePoint = Vector3.zero;

            foreach (ContactPoint contact in contactPoints)
            {
                middlePoint += contact.point;
            }

            middlePoint /= contactPoints.Length;

            _crashParticleEffect.transform.position = middlePoint;
            _crashParticleEffect.Play();
        }
    }

    private void RestartCar()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        transform.position = _startingPosition;
        transform.rotation = Quaternion.Euler(_startingRotation);
    }

    private void OnCollisionEnter(Collision other)
    {
        StartCoroutine(Collision(other));
    }
}
