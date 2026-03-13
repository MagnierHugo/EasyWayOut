using UnityEngine;

public class ShotGun : Gun {
    private MagManager magManager = new MagManager();
    private int RemainingBulletsToAdd = 2;

    void Start() {
        magManager.InitMag(8);
    }

    public void ManualyLoadBullet() {
        if (RemainingBulletsToAdd > 0) {
            magManager.AddBullet();
            magManager.Shuffle();
            RemainingBulletsToAdd--;
        }
    }

    public override void Shoot(IShootable target) {
        if (target == null) return;
        if (magManager.GetBullet()) {
            target.GetShot();
        } else {
            magManager.ShootBullet();
            target.EmptyShot();
        }
    }
}