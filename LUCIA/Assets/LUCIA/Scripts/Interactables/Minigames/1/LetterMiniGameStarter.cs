using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class LetterMiniGameStarter : MonoBehaviour
{
    [SerializeField] private GameObject pressE_Hint; 

    private bool isPlayerInZone = false;

    private void Start()
    {
        if(pressE_Hint != null) pressE_Hint.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if(pressE_Hint != null) pressE_Hint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if(pressE_Hint != null) pressE_Hint.SetActive(false);
        }
    }

    private void Update()
    {
        // Verificar que el manager existe y el juego NO ha empezado aun
        if (LetterMiniGameManager.Instance == null || LetterMiniGameManager.Instance.IsGameActive) return;

        if (isPlayerInZone) 
        {
            // Detección de Input (Compatible con New Input System y Old)
            bool ePressed = false;
            
            #if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ePressed = true;
            #else
            if (Input.GetKeyDown(KeyCode.E)) ePressed = true;
            #endif

            if (ePressed)
            {
                LetterMiniGameManager.Instance.StartMiniGame();
                
                if(pressE_Hint != null) pressE_Hint.SetActive(false); 
                
                // Opcional: Desactivar este starter para que no se pueda volver a usar
                // gameObject.SetActive(false); 
            }
        }
    }
}