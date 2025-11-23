using UnityEngine;

public class LetterMiniGameItem : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Escribe aquí la palabra que aparecerá en pantalla al recoger este objeto")]
    [SerializeField] private string wordToDisplay = "PALABRA"; // <--- AQUÍ ESCRIBES EN EL INSPECTOR

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (LetterMiniGameManager.Instance != null && LetterMiniGameManager.Instance.IsGameActive)
            {
                // Enviamos el nombre de la palabra al manager
                LetterMiniGameManager.Instance.CollectItem(wordToDisplay);
                
                Destroy(gameObject); 
            }
        }
    }
}