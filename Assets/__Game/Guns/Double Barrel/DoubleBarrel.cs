using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoubleBarrel : Gun, IHaveSpecial
{
    [SerializeField] private Mag[] mags = new Mag[2];
    private int currentMag = 0;
    private IShootable lastSwitch;

    [SerializeField] private ParticleSystem muzzleFlash;

    private void Start()
    {
        Debug.Log("Double");

        foreach (Mag mag in mags)

            mag.InitMag(6);
        
        mag = mags[currentMag];
    }

    public override void Shoot(IShootable target)
    {
        if (mags[currentMag].GetBullet())
        {
            Debug.Log("BANG!");
            target.GetShot();
            muzzleFlash.Play();
        }
        else
        {
            Debug.Log("Click.");
            target.EmptyShot();
        }       
    }

    public void Special(IShootable target)
    {
        if (lastSwitch == target)
            return;

        currentMag = currentMag == 0 ? 1 : 0;
        lastSwitch = target;
    }

    public Mag GetOtherMag()
    {
        if (currentMag == 0)
            return mags[1];
        else
            return mags[0];
    }
}
