using Unity.VisualScripting;
using UnityEngine;

public class Burst : MonoBehaviour, IShoot, IHaveSpecial
{
    private readonly MagManager magManager = new MagManager();

    public bool canTargetOpponent = false;

    private void Start() => magManager.Init(10);

    public void Shoot(IShootable target)
    {
        if (magManager.NextBulletIsLive())
            target.GetShot();
    }

    public void Special(IShootable target)
    {
        Shoot(target);
        Shoot(target);
    }
}
