using System;
using UnityEditor;
using UnityEngine;

using UnityEngine.UI;
#if !UNITY_EDITOR
using static UnityEngine.Application;
#endif


public sealed class QuitGame : MonoBehaviour
{
    private Button button;
    private void Awake() => button = GetComponent<Button>();
    private void OnEnable() => button.onClick.AddListener(Quit);
    private void OnDisable() => button.onClick.RemoveListener(Quit);
#if UNITY_EDITOR
    private void Quit() => EditorApplication.ExitPlaymode();
#endif
}
