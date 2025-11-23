using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string videoFileName = "VideoInteraccion1.mp4";
    public string VideoFileName => videoFileName; // Propiedad pública para leer el nombre

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionManager.Instance.RegisterInteractable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionManager.Instance.UnregisterInteractable(this);
        }
    }
}