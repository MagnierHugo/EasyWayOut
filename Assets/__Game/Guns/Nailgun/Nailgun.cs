using UnityEngine;

public class Nailgun : MonoBehaviour, IShoot, IHaveSpecial
{
    private MagManager magManager;

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


    public void Shoot(Player target)
    {
        if (magManager.GetBullet())
        {
            target.GetShot();
        }

        if(!canShootHand && !isHandNailed) { canShootHand = true;}
    }

    public void Special(Player target)
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
