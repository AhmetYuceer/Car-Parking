using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
     public static GameManager Instance;

     [SerializeField] List<CarController> cars = new List<CarController>();
     
     public bool IsPlay = true;
     [SerializeField] private int MovesCount = 0;

     private void Awake()
     {
          if (Instance == null)
               Instance = this;
          else
               Destroy(gameObject);
     }

     private void Start()
     {
          SoundManager.Instance.PlayBackgroundMusic();
          UIManager.Instance.SetMoveCountText(MovesCount);
          IsPlay = true;
     }

     public void CheckLevel()
     {
          foreach (var car in cars)
          {
               if (!car.IsFinish)
                    return;
          }
          WonGame();
     }
     
     public IEnumerator EndGame()
     {
          IsPlay = false;
          yield return new WaitForSeconds(1f);
          UIManager.Instance.ShowLosePanel();          
          SoundManager.Instance.PlayLevelFailedSfx();
     }

     private void WonGame()
     {
          IsPlay = false;
          UIManager.Instance.ShowWinPanel();
          SoundManager.Instance.PlayLevelComplateSfx();
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
          UIManager.Instance.SetMoveCountText(MovesCount);
     }
}
