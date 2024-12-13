using Interface;
using UnityEngine;

public class CarController : MonoBehaviour , IMoveable
{
     [SerializeField] private Direction _direction;
 
     public void Move(Direction direction)
     {
          Debug.Log(gameObject.name + " - " + direction);
     }
}