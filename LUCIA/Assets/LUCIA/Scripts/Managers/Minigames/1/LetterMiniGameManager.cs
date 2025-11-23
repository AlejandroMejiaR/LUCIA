using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class LetterMiniGameManager : MonoBehaviour
{
    public static LetterMiniGameManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject hudPanel;       
    [SerializeField] private TextMeshProUGUI infoText; 
    [SerializeField] private GameObject victoryPanel;   
    [SerializeField] private Button closeVictoryButton;

    [Header("Game Config")]
    [SerializeField] private int totalItemsToCollect = 5;

    [Header("Audio SFX")]
    [SerializeField] private AudioSource sfxSource; // Arrastra el AudioSource
    [SerializeField] private AudioClip collectItemSound;
    [SerializeField] private AudioClip victorySound;

    private int currentCollectedCount = 0;
    private List<string> collectedWordsList = new List<string>();
    private bool isGameActive = false;

    public bool IsGameActive => isGameActive; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); 
        
        if(closeVictoryButton != null)
            closeVictoryButton.onClick.AddListener(CloseVictoryPanel);
    }

    private void Start()
    {
        if(hudPanel != null) hudPanel.SetActive(false);
        if(victoryPanel != null) victoryPanel.SetActive(false);
        isGameActive = false;
    }

    public void StartMiniGame()
    {
        if (isGameActive) return;

        isGameActive = true;
        currentCollectedCount = 0;
        collectedWordsList.Clear();
        
        if(hudPanel != null) hudPanel.SetActive(true);
        UpdateUI();
    }

    public void CollectItem(string wordName)
    {
        if (!isGameActive) return;

        currentCollectedCount++;
        collectedWordsList.Add(wordName);
        UpdateUI();

        // SONIDO ITEM
        if (sfxSource != null && collectItemSound != null)
            sfxSource.PlayOneShot(collectItemSound);

        if (currentCollectedCount >= totalItemsToCollect)
        {
            WinGame();
        }
    }

    private void UpdateUI()
    {
        if(infoText != null)
        {
            string wordsString = string.Join(" - ", collectedWordsList);
            infoText.text = $"Palabras ({currentCollectedCount}/{totalItemsToCollect}):\n<color=yellow>{wordsString}</color>";
        }
    }

    private void WinGame()
    {
        isGameActive = false;
        if(hudPanel != null) hudPanel.SetActive(false); 
        if(victoryPanel != null) victoryPanel.SetActive(true); 
        
        // SONIDO VICTORIA
        if (sfxSource != null && victorySound != null)
            sfxSource.PlayOneShot(victorySound);

        if(GameManager.Instance != null) 
            GameManager.Instance.UnlockCursor();
    }

    private void CloseVictoryPanel()
    {
        if(victoryPanel != null) victoryPanel.SetActive(false);
        if(GameManager.Instance != null) GameManager.Instance.LockCursor(); 
    }
}