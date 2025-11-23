using UnityEngine;

public class CollectableLetter : MonoBehaviour
{
    [SerializeField] private char letterToGive = 'A';

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WordCollectionManager.Instance.CollectLetter(letterToGive);
            Destroy(gameObject); // Desaparece la letra
        }
    }
}