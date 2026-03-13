using UnityEngine;

public class ShotGun : Gun {
    private Mag mag = new Mag();
    private int RemainingBulletsToAdd = 2;

    void Start() {
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
        if (mag.GetBullet()) {
            target.GetShot();
        } else {
            mag.ShootBullet();
        }
    }
}