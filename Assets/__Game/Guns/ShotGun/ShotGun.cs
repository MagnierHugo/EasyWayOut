using UnityEngine;

public class ShotGun : MonoBehaviour, IShoot {
    private MagManager magManager;
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
    public void Shoot(Player target) {
        if (target == null) return;
        if (magManager.GetBullet()) {
            target.GetShot();
        } else {
            magManager.ShootBullet();
        }
    }
}