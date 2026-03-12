using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PauseMenuManager : MonoBehaviour
{
    private InputAction escAction;
    [SerializeField] private GameObject pauseMenuCanvas;

    private void Awake()
    {
        escAction = new InputAction(binding: "<Keyboard>/escape");
        escAction.performed += TogglePause;
    }
    private void OnEnable() => escAction.Enable();
    private void OnDisable() => escAction.Disable();

    private void TogglePause(InputAction.CallbackContext _)
    {
        if (pauseMenuCanvas.activeSelf)
        {
            pauseMenuCanvas.SetActive(false);
            Game.SwitchToMenu();
        }
        else
        {
            pauseMenuCanvas.SetActive(true);
            Game.SwitchToGameplay();
        }
    }
}
