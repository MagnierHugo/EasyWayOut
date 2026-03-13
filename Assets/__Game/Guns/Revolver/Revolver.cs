using UnityEngine;

public class Revolver : Gun, IHaveSpecial
{
    private Mag magManager;

    private void Start()
    {
        magManager.InitMag(6);
    }

    public override void Shoot(IShootable target)
    {
        if(magManager.GetBullet())
        {
            target.GetShot();
        }
    }

    public void Special(IShootable target)
    {
        magManager.AddBullet();
        magManager.Shuffle();
        Shoot(target);
    }
}
