using System;
using System.Collections;
using System.Collections.Generic;


using UnityEngine;


public sealed class Game : MonoBehaviour
{
    static Game()
    {
        if (Manager != null)
            return;

        Manager = new GameObject("Game").AddComponent<Game>();
    }
    public static Game Manager { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        OnSwitchToGameplay += ResumeTime;
        OnSwitchToMenu += StopTime;

        Manager = this;

        SwitchToGameplay();
    }

    private void OnDestroy()
    {
        OnSwitchToGameplay -= ResumeTime;
        OnSwitchToMenu -= StopTime;
    }


    private void StopTime() => Time.timeScale = 0f;
    private void ResumeTime() => Time.timeScale = 1f;

    public static event Action OnSwitchToGameplay;
    public static void SwitchToGameplay() => OnSwitchToGameplay?.Invoke();

    public static event Action OnSwitchToMenu;
    public static void SwitchToMenu() => OnSwitchToMenu?.Invoke();
}