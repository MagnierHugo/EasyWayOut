using UnityEngine;

public class Nailgun : Gun, IHaveSpecial
{
    private MagManager magManager = new MagManager();

    public bool canTargetOpponent = false;

    private bool canShootHand = true;
    private bool isHandNailed = false;

    private void Start()
    {
        // Init mag with 11 chambers and 3 bullets
        magManager.InitMag(11);
        magManager.AddBullet();
        magManager.Shuffle();
        magManager.AddBullet();
        magManager.Shuffle();
    }


    public override void Shoot(IShootable target)
    {
        if (magManager.GetBullet()) target.GetShot();
        else target.EmptyShot();


        if (!canShootHand && !isHandNailed) { canShootHand = true; }
    }

    public void Special(IShootable target)
    {
        if (canShootHand)
        {
            if (magManager.GetBullet())
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
