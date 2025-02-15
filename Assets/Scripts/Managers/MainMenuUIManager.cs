using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private Button _startGameButton , _exitGameButton;
    
    [SerializeField] private GameObject _levelsPanel;
    [SerializeField] private Button _level1, _level2, _level3, _level4, _level5, _level6;
    [SerializeField] private Button _backButton;

    private void Start()
    {
        _mainMenu.SetActive(true);
        _levelsPanel.SetActive(false);
        
        _startGameButton.onClick.AddListener(() =>
        {
            _mainMenu.SetActive(false);
            _levelsPanel.SetActive(true);
        });      

        _exitGameButton.onClick.AddListener(Application.Quit);
        
        _backButton.onClick.AddListener(() =>
        {
            _mainMenu.SetActive(true);
            _levelsPanel.SetActive(false);
        });  
        
        _level1.onClick.AddListener(() => SceneLoaderManager.Instance.LoadScene(1));
        _level2.onClick.AddListener(() => SceneLoaderManager.Instance.LoadScene(2));
        _level3.onClick.AddListener(() => SceneLoaderManager.Instance.LoadScene(3));
        _level4.onClick.AddListener(() => SceneLoaderManager.Instance.LoadScene(4));
        _level5.onClick.AddListener(() => SceneLoaderManager.Instance.LoadScene(5));
        _level6.onClick.AddListener(() => SceneLoaderManager.Instance.LoadScene(6));
    }
}
