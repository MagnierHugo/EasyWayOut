using Unity.VisualScripting;
using UnityEngine;

public class Burst : Gun, IHaveSpecial
{
    private Mag mag = new Mag();

    public bool canTargetOpponent = false;

    private void Start()
    {
        mag.InitMag(10);
    }

    public override void Shoot(IShootable target)
    {
        if (mag.GetBullet()) target.GetShot();
        else target.EmptyShot();
    }

    public void Special(IShootable target)
    {
        Shoot(target);
        Shoot(target);
    }
}
