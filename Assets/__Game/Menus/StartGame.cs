using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class StartGame : MonoBehaviour {
    [SerializeField] private GameObject Menu;
    [SerializeField] private Button button;
    private void OnEnable() {
        // button.onClick.AddListener(LoadGameScene);
        button.onClick.AddListener(Game.SwitchToGameplay);
    }

    private void OnDisable() {
        // button.onClick.RemoveListener(LoadGameScene);
        button.onClick.RemoveListener(Game.SwitchToGameplay);
    }
    
    public void ToggleExitFromScript() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #endif

        Application.Quit();
    }

    public void LoadGameScene() {
        UnityEngine.Debug.Log("Hidden Menu...");
        Menu.SetActive(false);
    }
}