using UnityEngine;

public class Revolver : MonoBehaviour, IShoot, IHaveSpecial
{
    private MagManager magManager;

    private void Start()
    {
        magManager.InitMag(6);
    }

    public void Shoot(IShootable target)
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
