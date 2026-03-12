using UnityEngine;
using UnityEngine.UI;

public sealed class ResumeGame : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;
    private Button button;
    private void Awake() => button = GetComponent<Button>();
    private void OnEnable()
    {
        button.onClick.AddListener(Resume);
        button.onClick.AddListener(Game.SwitchToGameplay);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Resume);
        button.onClick.RemoveListener(Game.SwitchToGameplay);
    }

    private void Resume() => pauseMenuCanvas.SetActive(false);
}
