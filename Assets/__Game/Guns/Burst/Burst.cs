using Unity.VisualScripting;
using UnityEngine;

public class Burst : Gun, IHaveSpecial
{
    private Mag magManager;

    public bool canTargetOpponent = false;

    private void Start()
    {
        magManager.InitMag(10);
    }

    public override void Shoot(IShootable target)
    {
        if (magManager.GetBullet())
        {
            target.GetShot();
        }
    }

    public void Special(IShootable target)
    {
        Shoot(target);
        Shoot(target);
    }
}
