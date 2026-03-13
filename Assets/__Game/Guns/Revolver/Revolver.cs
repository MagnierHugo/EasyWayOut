using UnityEngine;

public class Revolver : Gun, IHaveSpecial
{
    private Mag mag = new Mag();

    private void Awake()
    {
        Debug.Log("Revolver");
        mag.InitMag(6);
    }

    public override void Shoot(IShootable target)
    {
        bool hasBullet = mag.GetBullet();

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
        mag.AddBullet();
        mag.Shuffle();
        Shoot(target);
    }
}
