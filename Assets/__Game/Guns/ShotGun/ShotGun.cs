using UnityEngine;

public class ShotGun : Gun {
    private Mag magManager;
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

    public override void Shoot(IShootable target)
    {
        if (magManager.GetBullet()) {
            target.GetShot();
        }
    }
}