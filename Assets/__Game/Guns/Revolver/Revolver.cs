using UnityEngine;

public class Revolver : MonoBehaviour, IShoot, IHaveSpecial
{
    private readonly MagManager magManager = new MagManager();

    private void Start() => magManager.Init(6);

    public void Shoot(IShootable target)
    {
        if(magManager.NextBulletIsLive())
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
