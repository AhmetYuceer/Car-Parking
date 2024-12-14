using UnityEngine;

public class GameManager : MonoBehaviour
{
     public static GameManager Instance;
     
     [SerializeField] private int MovesCount = 0;

     private void Awake()
     {
          if (Instance == null)
               Instance = this;
          else
               Destroy(gameObject);
     }

     public bool CheckMove()
     {
          if (MovesCount > 0)
               return true;
          
          return false;
     }

     public void Move(int moveAmount)
     {
          MovesCount -= moveAmount;
     }
}
