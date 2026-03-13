using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class StartGame : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    private Button button;
    private void Awake() => button = GetComponent<Button>();
    private void OnEnable()
    {
        button.onClick.AddListener(LoadGameScene);
        button.onClick.AddListener(Game.SwitchToGameplay);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(LoadGameScene);
        button.onClick.RemoveListener(Game.SwitchToGameplay);
    }

    private void LoadGameScene() => SceneManager.LoadScene(gameSceneName);
}