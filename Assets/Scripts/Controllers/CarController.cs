using System;
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
    
    private bool _isMovable = true;
    public bool IsFinish = false;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _isMoving = false;
        _isMovable = true;
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
        if (!_isMoving && _isMovable)
        {
            if (StartMoving(direction))
                GameManager.Instance.Move(1);
        }
    }

    public void TrafficLightMove(Direction direction)
    {
        if (!_isMoving && _isMovable)
            StartMoving(direction);
    }

    private void MoveForward()
    {
        Vector3 step = _movementDirection * (_speed * Time.fixedDeltaTime);
        Vector3 newPosition = transform.position + step;
        _rigidbody.MovePosition(newPosition);
    }

    private bool StartMoving(Direction direction)
    {
        foreach (var nothingDirection in _unavailableDirections)
        {
            if (nothingDirection == direction)
                return false;
        }        
        
        switch (direction)
        {
            case Direction.Forward:
                _movementDirection = Vector3.forward;
                break;
            case Direction.Backward:
                _movementDirection = Vector3.back;
                break;
            case Direction.Right:
                _movementDirection = Vector3.right;
                break;
            case Direction.Left:
                _movementDirection = Vector3.left;
                break;
        }

        if (!_smokeParticle.isPlaying)
            _smokeParticle.Play();

        _isMoving = true;
        SoundManager.Instance.PlayCarStartingSound();
        return true;
    }

    private void StopMoving()
    {
        _isMoving = false;
        _smokeParticle.Stop();
        _movementDirection = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private IEnumerator Collision(Collision collision)
    {
        Crash(collision);
        yield return new WaitForSeconds(3f);
        //RestartCar();
    }

    private void Crash(Collision collision)
    {
        SoundManager.Instance.PlayCrashSfx();
        CrashParticle(collision);
        DOTween.Kill("Rotate");
        StopMoving();
        StartCoroutine(GameManager.Instance.EndGame());
    }    
    private void Finish()
    {
        IsFinish = true;
        StopMoving();
        GetComponent<BoxCollider>().isTrigger = true;
        _rigidbody.useGravity = false;
        GameManager.Instance.CheckLevel();
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Car") || other.collider.CompareTag("Obstacle"))
            StartCoroutine(Collision(other));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
            Finish();
    }
}
