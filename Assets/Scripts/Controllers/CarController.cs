using System.Collections;
using UnityEngine;
using Interface;
using System.Collections.Generic;
using DG.Tweening;

public class CarController : MonoBehaviour , IMoveable
{
     [SerializeField] private List<PathPiece> _paths = new List<PathPiece>();
     [SerializeField] private Direction _targetDirection;
     [SerializeField] private bool _isMoving;
     [SerializeField] private ParticleSystem _smokeParticle;
     [SerializeField] private ParticleSystem _crashParticleEffect;
     private PathPiece _targetPiece;
     private float _speed = 10f;     
     private int pathIndex = 0;
     private Rigidbody _rigidbody;
     private Vector3 _startingPosition;
     private Vector3 _startingRotation;
     
     private void Start()
     {
          _rigidbody = GetComponent<Rigidbody>();
          _isMoving = false;
          pathIndex = 0;
          _startingPosition = transform.position;
          _startingRotation = transform.rotation.eulerAngles;
     }

     private void FixedUpdate()
     {
          if (_isMoving)
          {
               Moving();
          }
     }
     
     public void Move(Direction direction)
     {
          if (_paths.Count != 0)
          {
               if (_targetDirection == direction)
               {
                    SetPiece();
               }
          }
     }
     
     private void Moving()
     {
          if (_targetPiece != null)
          {
               Vector3 direction = (_targetPiece.transform.position - transform.position).normalized;
               Vector3 step = direction * (_speed * Time.fixedDeltaTime);
               Vector3 newPosition = transform.position + step;
               _rigidbody.MovePosition(newPosition);

               if (Vector3.Distance(transform.position, _targetPiece.transform.position) <= 0.1f)
               { 
                    transform.position = _targetPiece.transform.position;
                    _isMoving = false;
                    Rotate();
                    NextPiece();
               }
          }
     }
     
     private void Rotate()
     {
          if (_targetPiece != null)
               transform.DORotate(_targetPiece.Rotation, 0.5f).SetId("Rotate");
     }
      
     private void SetPiece()
     {
          if (!_smokeParticle.isPlaying)
               _smokeParticle.Play();
          
          _targetPiece = _paths[pathIndex];
          _isMoving = true;
     }

     private void NextPiece()
     {
          pathIndex++;
          
          if (pathIndex <= _paths.Count - 1)
               SetPiece();
          else
               pathIndex = 0;
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
          _smokeParticle.Stop();
          _isMoving = false;
          _targetPiece = null;
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
          pathIndex = 0;
          _rigidbody.velocity = Vector3.zero;
          _rigidbody.angularVelocity = Vector3.zero;
          transform.position = _startingPosition;
          transform.rotation = Quaternion.Euler(_startingRotation.x, _startingRotation.y, _startingRotation.z);
     }
     
     private void OnCollisionEnter(Collision other)
     {
          StartCoroutine(Collision(other));
     }
}