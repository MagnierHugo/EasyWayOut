using Unity.VisualScripting;
using UnityEngine;

public class Burst : MonoBehaviour, IShoot, IHaveSpecial
{
    private MagManager magManager;

    public bool canTargetOpponent = false;

    private void Start()
    {
        magManager.InitMag(10);
    }

    public void Shoot(IShootable target)
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
