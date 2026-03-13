using Unity.VisualScripting;
using UnityEngine;

public class Busrt : MonoBehaviour, IShoot, IHaveSpecial
{
    private MagManager magManager;

    public bool canTargetOpponent = false;

    private void Start()
    {
        magManager.InitMag(10);
    }

    public void Shoot(Player target)
    {
        if (magManager.GetBullet())
        {
            target.GetShot();
        }
    }

    public void Special(Player target)
    {
        Shoot(target);
        Shoot(target);
    }
}
