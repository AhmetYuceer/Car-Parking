using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
     public static SceneLoaderManager Instance;
     private void Awake()
     {
          if (Instance == null)
          {
               Instance = this;
               DontDestroyOnLoad(this.gameObject);
          }
          else
          {
               Destroy(gameObject);
          }
     }

     public void LoadScene(int sceneIndex)
     {
          SceneManager.LoadScene(sceneIndex);          
     }
}
