using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    private PlayerControls controls;

    [SerializeField] private Player player;

    private Target target;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Player.Scroll.performed += OnScrollPerformed;
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Scroll.performed -= OnScrollPerformed;
    }

    private void OnScrollPerformed(InputAction.CallbackContext context)
    {
        float scrollDelta = context.ReadValue<Vector2>().y;

        if (scrollDelta == 0) return;
        else if (scrollDelta > 0) target = (Target)Mathf.Clamp01((int)(target += 1));
        else target = (Target)Mathf.Clamp((int)(target -= 1), -1, 0);

        player.ChangeTarget(target);
    }
}


public enum Target
{
    None = 0,
    Self = -1,
    Opponent = 1
}
