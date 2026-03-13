using UnityEngine;

public class Revolver : Gun, IHaveSpecial
{
    private MagManager magManager = new MagManager();

    private void Awake()
    {
        magManager.InitMag(6);
    }

    public override void Shoot(IShootable target)
    {
        bool hasBullet = magManager.GetBullet();

        // 3. Resolve the shot
        if (hasBullet)
        {
            Debug.Log("BANG!");
            target.GetShot();
        }
        else
        {
            Debug.Log("Click.");
            target.EmptyShot();
        }
    }

    public void Special(IShootable target)
    {
        magManager.AddBullet();
        magManager.Shuffle();
        Shoot(target);
    }
}
