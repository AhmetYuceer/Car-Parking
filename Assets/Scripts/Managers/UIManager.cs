using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private int _nextLevelIndex;
    [SerializeField] private TextMeshProUGUI _moveCountText;
    
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private Button _winPanelRestartButton,_winPanelMenuButton, _nextLevelButton;

    [SerializeField] private GameObject _losePanel;
    [SerializeField] private Button _losePanelRestartButton,_losePanelMenuButton;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
        
        _winPanelRestartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }); 
        
        _winPanelMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });

        _nextLevelButton.onClick.AddListener(() => SceneLoaderManager.Instance.LoadScene(_nextLevelIndex));
        
        _losePanelRestartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }); 
        
        _losePanelMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }

    public void ShowWinPanel()
    {
        _winPanel.SetActive(true);
        _losePanel.SetActive(false);
    }
    
    public void ShowLosePanel()
    {
        _winPanel.SetActive(false);
        _losePanel.SetActive(true);
    }
    
    public void SetMoveCountText(int moveCount)
    {
        _moveCountText.text = moveCount.ToString();
    }
}
