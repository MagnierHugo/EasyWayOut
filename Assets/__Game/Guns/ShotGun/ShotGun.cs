using UnityEngine;

public class ShotGun : Gun
{
    private int RemainingBulletsToAdd = 2;

    void Start() {
        Debug.Log("Shotgun");
        mag.InitMag(8);
    }

    public void ManualyLoadBullet() {
        if (RemainingBulletsToAdd > 0) {
            mag.AddBullet();
            mag.Shuffle();
            RemainingBulletsToAdd--;
        }
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
    }
}