using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Configuración")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private string volumeParameter = "MasterVolume";

    private void Awake()
    {
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
        // Convierte 0-1 a Decibeles (-80 a 0)
        float volume = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20;
        mainMixer.SetFloat(volumeParameter, volume);
    }

    // --- NUEVA FUNCIÓN ---
    public float GetCurrentVolume()
    {
        // Preguntamos al Mixer a cuántos decibeles está
        bool result = mainMixer.GetFloat(volumeParameter, out float dbValue);
        
        if (result)
        {
            // Convertimos Decibeles de vuelta a 0-1 para el slider
            // Fórmula inversa: 10 ^ (db / 20)
            return Mathf.Pow(10, dbValue / 20);
        }
        else
        {
            return 1f; // Si falla, asumimos volumen máximo
        }
    }
}