using Unity.VisualScripting;
using UnityEngine;

public class Burst : Gun, IHaveSpecial
{
    private MagManager magManager = new MagManager();

    public bool canTargetOpponent = false;

    private void Start()
    {
        magManager.InitMag(10);
    }

    public override void Shoot(IShootable target)
    {
        if (magManager.GetBullet()) target.GetShot();
        else target.EmptyShot();
    }

    public void Special(IShootable target)
    {
        Shoot(target);
        Shoot(target);
    }

    public void Special()
    {
        throw new System.NotImplementedException();
    }
}
