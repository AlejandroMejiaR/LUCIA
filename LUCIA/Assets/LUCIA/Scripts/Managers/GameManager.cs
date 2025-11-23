using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Estado para saber si estamos viendo un video o jugando
    public bool IsInputLocked { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Al iniciar la escena de juego, bloqueamos el cursor para FPS
        LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsInputLocked = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        IsInputLocked = true; // Bloqueamos input de movimiento del personaje
    }
}