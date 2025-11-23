using UnityEngine;
using TMPro;

public class WordCollectionManager : MonoBehaviour
{
    public static WordCollectionManager Instance;

    [Header("UI Config")]
    [SerializeField] private TextMeshProUGUI wordDisplayUI;
    [SerializeField] private string targetWord = "MULTIMEDIA";
    [SerializeField] private Color winColor = Color.green; 

    [Header("Audio SFX")]
    [SerializeField] private AudioSource sfxSource; // Arrastra el AudioSource aquí
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip completeSound;

    private char[] currentWordState;
    private int lettersRevealed = 0; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        currentWordState = new char[targetWord.Length];
        for (int i = 0; i < currentWordState.Length; i++) currentWordState[i] = '_';
        UpdateUI();
    }

    public void CollectLetter(char letter)
    {
        letter = char.ToUpper(letter);
        bool foundAny = false;

        for (int i = 0; i < targetWord.Length; i++)
        {
            if (targetWord[i] == letter && currentWordState[i] == '_')
            {
                currentWordState[i] = letter;
                lettersRevealed++;
                foundAny = true;
            }
        }

        if (foundAny)
        {
            UpdateUI();
            
            // REPRODUCIR SONIDO
            if (sfxSource != null && collectSound != null)
                sfxSource.PlayOneShot(collectSound);

            CheckWinCondition();
        }
    }

    private void UpdateUI()
    {
        wordDisplayUI.text = string.Join(" ", currentWordState);
    }

    private void CheckWinCondition()
    {
        if (lettersRevealed >= targetWord.Length)
        {
            wordDisplayUI.color = winColor;
            
            // SONIDO DE VICTORIA
            if (sfxSource != null && completeSound != null)
                sfxSource.PlayOneShot(completeSound);
        }
    }
}