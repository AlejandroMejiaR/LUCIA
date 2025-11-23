using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class ElevatorController : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Transform targetPosition; 
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private GameObject buttonObject;  
    [SerializeField] private float interactionDistance = 3.0f; 

    [Header("UI Opcional")]
    [SerializeField] private GameObject pressE_Hint;

    [Header("Audio")]
    [SerializeField] private AudioSource elevatorAudioSource; // Pon un AudioSource AQUÍ en el ascensor

    private bool isMoving = false;
    private bool hasArrived = false;
    private Transform playerTransform; 

    private void Update()
    {
        if (!isMoving && !hasArrived) CheckButtonInput();
        if (isMoving && !hasArrived) MoveElevator();
    }

    private void CheckButtonInput()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
            return;
        }

        float distance = Vector3.Distance(playerTransform.position, buttonObject.transform.position);

        if (distance <= interactionDistance)
        {
            if (pressE_Hint != null) pressE_Hint.SetActive(true);

            bool ePressed = false;
            #if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ePressed = true;
            #else
            if (Input.GetKeyDown(KeyCode.E)) ePressed = true;
            #endif

            if (ePressed)
            {
                StartMoving(); // Llamamos a función dedicada
            }
        }
        else
        {
            if (pressE_Hint != null) pressE_Hint.SetActive(false);
        }
    }

    private void StartMoving()
    {
        isMoving = true;
        if (pressE_Hint != null) pressE_Hint.SetActive(false);

        // REPRODUCIR SONIDO (Loop)
        if (elevatorAudioSource != null)
        {
            elevatorAudioSource.loop = true;
            elevatorAudioSource.Play();
        }
    }

    private void MoveElevator()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition.position) < 0.01f)
        {
            hasArrived = true;
            isMoving = false;
            
            // DETENER SONIDO
            if (elevatorAudioSource != null)
            {
                elevatorAudioSource.Stop();
            }
            
            Debug.Log("Ascensor llegó al destino");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) other.transform.SetParent(this.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
            DontDestroyOnLoad(other.gameObject); 
        }
    }
}