using UnityEngine;

public sealed class Shotgun : MonoBehaviour, IShoot
{
    public static Shotgun Instance { get; private set; }

    private readonly MagManager magManager = new MagManager();
    private void Awake() => Instance = this;

    public void LoadShells()
    {
        magManager.Init(8);

        //if (remainingBulletsToAdd > 0)
        //{
        //    magManager.AddBullet();
        //    magManager.Shuffle();
        //    remainingBulletsToAdd--;
        //}
    }

    public void Shoot(IShootable target)
    {
        if (target == null)
            return;

        if (magManager.NextBulletIsLive())
        {
            target.GetShot();
        }
        else
        {
            magManager.ShootBullet();
        }
    }
}