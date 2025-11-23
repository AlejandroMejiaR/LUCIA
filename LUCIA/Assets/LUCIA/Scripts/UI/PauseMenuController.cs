using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private string mainMenuSceneName = "MainMenuScene"; 

    private bool isPaused = false;

    private void Start()
    {
        pausePanel.SetActive(false);

        // CONECTAR CON EL SISTEMA DE AUDIO
        if (AudioManager.Instance != null)
        {
            // 1. SINCRONIZAR VISUALMENTE: 
            // Pedimos el valor actual y movemos la perilla del slider a esa posición
            volumeSlider.value = AudioManager.Instance.GetCurrentVolume();

            // 2. CONECTAR FUNCIONALIDAD:
            volumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetVolume);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;

        if (GameManager.Instance != null) GameManager.Instance.LockCursor();
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true;

        if (GameManager.Instance != null) GameManager.Instance.UnlockCursor();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        if (SceneLoaderManager.Instance != null) SceneLoaderManager.Instance.LoadScene(mainMenuSceneName);
        else SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}