using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoubleBarrel : Gun, IHaveSpecial
{
    [SerializeField] private Mag[] mags = new Mag[2];
    private int currentMag = 0;
    private IShootable lastSwitch;

    [SerializeField] private ParticleSystem muzzleFlash;

    public override void Shoot(IShootable target)
    {
        bool bullet = mags[currentMag].GetBullet();
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
