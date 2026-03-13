using Unity.VisualScripting;
using UnityEngine;

public class Burst : Gun, IHaveSpecial
{
    private readonly Mag mag = new Mag();

    public bool canTargetOpponent = false;

    private void Start() => mag.Init(10);

    public override void Shoot(IShootable target)
    {

        if (mag.NextBulletIsLive())
            target.GetShot();
        else
            target.EmptyShot();

    }

    public void Special(IShootable target)
    {
        Shoot(target);
        Shoot(target);
    }
}
