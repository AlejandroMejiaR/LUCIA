using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject videoPanel; // Panel que contiene el RawImage del video

    [Header("Video Config")]
    [SerializeField] private VideoPlayer introVideoPlayer;
    [SerializeField] private string introVideoFileName = "IntroVideo.mp4";
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("Configuración Settings")]
    [SerializeField] private Slider volumeSlider;
    [Header("Audio Config")]
    [SerializeField] private AudioSource menuMusicSource; // <--- NUEVA LINEA

    private void Start()
    {
        // Asegurar estado inicial
        ShowPanel(mainPanel);
        videoPanel.SetActive(false);

        // Configurar el slider si existe AudioManager
        if(AudioManager.Instance != null)
        {
            volumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetVolume);
        }
        
        // Suscribirse al evento de fin de video
        introVideoPlayer.loopPointReached += OnIntroVideoFinished;
    }

    // --- Funciones de Botones del Menú Principal ---

    public void OnPlayButtonClicked()
    {
        // 1. Detener la música del menú
        if (menuMusicSource != null)
        {
            menuMusicSource.Stop(); // <--- NUEVA LINEA: Adiós música, hola video
        }

        // 2. Ocultar menú
        mainPanel.SetActive(false);
        
        // 3. Activar panel de video
        videoPanel.SetActive(true);

        // 4. Configurar y reproducir video
        string videoPath = Path.Combine(Application.streamingAssetsPath, introVideoFileName);
        introVideoPlayer.source = VideoSource.Url;
        introVideoPlayer.url = videoPath;
        
        introVideoPlayer.Prepare();
        introVideoPlayer.Play();
    }

    public void OnCreditsButtonClicked()
    {
        ShowPanel(creditsPanel);
    }

    public void OnSettingsButtonClicked()
    {
        ShowPanel(settingsPanel);
    }

    public void OnBackToMenuClicked()
    {
        ShowPanel(mainPanel);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    // --- Lógica Interna ---

    private void ShowPanel(GameObject panelToShow)
    {
        mainPanel.SetActive(false);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        videoPanel.SetActive(false);

        panelToShow.SetActive(true);
    }

    private void OnIntroVideoFinished(VideoPlayer vp)
    {
        // Cuando el video termina, cargamos la escena de juego
        SceneLoaderManager.Instance.LoadScene(gameSceneName);
    }
}