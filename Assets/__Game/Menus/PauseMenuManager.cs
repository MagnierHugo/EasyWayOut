using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PauseMenuManager : MonoBehaviour {
    private InputAction escAction;
    [SerializeField] private GameObject pauseMenuCanvas;

    private void Awake() {
        escAction = new InputAction(binding: "<Keyboard>/escape");
        escAction.performed += TogglePause;
    }
    private void OnEnable() => escAction.Enable();
    private void OnDisable() => escAction.Disable();

    public void TogglePauseFromScript() {
        if (pauseMenuCanvas.activeSelf) {
            pauseMenuCanvas.SetActive(false);
            Game.SwitchToGameplay();
        } else {
            pauseMenuCanvas.SetActive(true);
            Game.SwitchToMenu();
        }
    }
    
    public void ToggleExitFromScript() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #endif
        Application.Quit();
    }

    private void TogglePause(InputAction.CallbackContext _) {
        if (pauseMenuCanvas.activeSelf) {
            pauseMenuCanvas.SetActive(false);
            Game.SwitchToGameplay();
        } else {
            pauseMenuCanvas.SetActive(true);
            Game.SwitchToMenu();
        }
    }
}
