using UnityEngine;

public class Nailgun : Gun, IHaveSpecial
{
    private Mag mag = new Mag();

    public bool canTargetOpponent = false;

    private bool canShootHand = true;
    private bool isHandNailed = false;

    private void Start()
    {
        Debug.Log("Nailgun");
        // Init mag with 11 chambers and 3 bullets
        mag.InitMag(11);
        mag.AddBullet();
        mag.Shuffle();
        mag.AddBullet();
        mag.Shuffle();
    }

    public override void Shoot(IShootable target)
    {
        if (mag.GetBullet())
        {
            Debug.Log("BANG!");
            target.GetShot();
        }
        else
        {
            Debug.Log("Click.");
            target.EmptyShot();
        }


        if (!canShootHand && !isHandNailed) { canShootHand = true; }
    }

    public void Special(IShootable target)
    {
        if (canShootHand)
        {
            if (mag.GetBullet())
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
