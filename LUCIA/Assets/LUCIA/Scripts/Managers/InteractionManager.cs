using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject pressE_Panel; // Panel flotante "Presiona E"
    [SerializeField] private GameObject videoDisplay_Panel; // Panel grande con el video
    [SerializeField] private RawImage videoScreen; // Donde se renderiza el video
    [SerializeField] private Button continueButton; // Botón para cerrar video

    [Header("Components")]
    [SerializeField] private VideoPlayer interactVideoPlayer;
    [SerializeField] private AudioSource videoAudioSource;

    private InteractableObject currentInteractable; // Objeto actual en rango
    private bool isVideoPlaying = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        // Configurar botón continuar
        continueButton.onClick.AddListener(CloseVideoInteraction);
    }

    private void Start()
    {
        pressE_Panel.SetActive(false);
        videoDisplay_Panel.SetActive(false);
    }

    private void Update()
    {
        // Si estamos viendo video, no detectar nada más
        if (isVideoPlaying) return;

        // Si hay un objeto cerca y presionamos E
        if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            StartVideoInteraction(currentInteractable.VideoFileName);
        }
    }

    // Llamado por el objeto cuando el jugador entra
    public void RegisterInteractable(InteractableObject obj)
    {
        currentInteractable = obj;
        pressE_Panel.SetActive(true);
    }

    // Llamado por el objeto cuando el jugador sale
    public void UnregisterInteractable(InteractableObject obj)
    {
        if (currentInteractable == obj)
        {
            currentInteractable = null;
            pressE_Panel.SetActive(false);
        }
    }

    private void StartVideoInteraction(string fileName)
    {
        isVideoPlaying = true;
        pressE_Panel.SetActive(false); 
        
        GameManager.Instance.UnlockCursor();
        
        // --- NUEVA LÓGICA DE LIMPIEZA ---
        
        // 1. Mostrar el panel
        videoDisplay_Panel.SetActive(true);

        // 2. Poner el RawImage en NEGRO (o transparente) temporalmente
        // Esto oculta el "fantasma" del video anterior
        videoScreen.color = Color.black; 

        // ---------------------------------

        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        
        interactVideoPlayer.source = VideoSource.Url;
        interactVideoPlayer.url = path;
        
        interactVideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        interactVideoPlayer.SetTargetAudioSource(0, videoAudioSource);

        Debug.Log("Cargando video: " + fileName);
        interactVideoPlayer.Prepare();

        // 3. Esperar a que esté listo para mostrar la imagen real
        interactVideoPlayer.prepareCompleted += (source) => 
        {
            // El video ya está listo para arrancar
            source.Play();

            // Volvemos a poner el RawImage en BLANCO (para que se vea la textura del video)
            videoScreen.color = Color.white; 
        };
    }

    private void CloseVideoInteraction()
    {
        interactVideoPlayer.Stop();
        videoDisplay_Panel.SetActive(false);
        
        // Volver al juego
        GameManager.Instance.LockCursor();
        isVideoPlaying = false;
        
        // Verificar si seguimos cerca del objeto para volver a mostrar "Presiona E"
        if(currentInteractable != null) pressE_Panel.SetActive(true);
    }
}