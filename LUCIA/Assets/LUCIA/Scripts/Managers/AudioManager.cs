using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Configuración")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private string volumeParameter = "MasterVolume";

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float sliderValue)
    {
        // Convertimos valor lineal del slider (0.0001 a 1) a decibeles logarítmicos (-80 a 0)
        // Mathf.Log10(sliderValue) * 20 es la fórmula estándar.
        // Nos aseguramos de que el slider nunca sea 0 absoluto para evitar errores matemáticos.
        float volume = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20;
        
        mainMixer.SetFloat(volumeParameter, volume);
    }
}