using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoubleBarrel : MonoBehaviour, IShoot, IHaveSpecial
{
    [SerializeField] private MagManager[] mags = new MagManager[2];
    private int currentMag = 0;
    private IShootable lastSwitch;

    [SerializeField] private ParticleSystem muzzleFlash;
    //[SerializeField] private InputActionAsset inputActionAsset;

    //private void OnEnable()
    //{
    //    InputAction action = inputActionAsset.FindActionMap("default").FindAction("Shoot");

    //    action.started += OnShoot;
    //    action.Enable();
    //}

    //private void OnDisable()
    //{
    //    InputAction action = inputActionAsset.FindActionMap("default").FindAction("Shoot");

    //    action.Disable();
    //    action.started -= OnShoot;
    //}

    private void OnShoot(InputAction.CallbackContext context)
    {
        muzzleFlash.Play();
    }

    public void Shoot(IShootable target)
    {
        bool bullet = mags[currentMag].NextBulletIsLive();
        if (!bullet) return;

        muzzleFlash.Play();
        target.GetShot();
    }

    public void Special(IShootable target)
    {
        if (lastSwitch == target) return;

        currentMag = currentMag == 0 ? 1 : 0;
        lastSwitch = target;
    }
}
