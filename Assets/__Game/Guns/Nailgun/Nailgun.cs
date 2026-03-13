using UnityEngine;

public class Nailgun : MonoBehaviour, IShoot, IHaveSpecial
{
    private readonly MagManager magManager = new MagManager();

    public bool canTargetOpponent = false;

    private bool canShootHand = true;
    private bool isHandNailed = false;

    private void Start()
    {
        // Init mag with 11 chambers and 3 bullets
        magManager.Init(11);
        magManager.AddBullet();
        magManager.Shuffle();
        magManager.AddBullet();
        magManager.Shuffle();
    }


    public void Shoot(IShootable target)
    {
        if (magManager.NextBulletIsLive())
        {
            target.GetShot();
        }

        if(!canShootHand && !isHandNailed) { canShootHand = true;}
    }

    public void Special(IShootable target)
    {
        if (canShootHand)
        {
            if (magManager.NextBulletIsLive())
            {
                // Call shoot Hand animation (Nail version)
                isHandNailed = true;
            }
            else
            {
                // Call shoot Hand animation (No nail version)
            }
        }
    }
}
