
using UnityEngine;

public class Burst : Gun, IHaveSpecial
{
    public bool canTargetOpponent = false;
    [SerializeField] private ParticleSystem muzzleFlash;

    private void Start() => mag.Init(10);

    public override void Shoot(IShootable target)
    {
        target.GetShot();

        //if (mag.GetBullet())
        //{
        //    muzzleFlash.Play();
        //    target.GetShot();
        //}
        //else
        //    target.EmptyShot(false);

    }

    public void Special(IShootable target)
    {
        Shoot(target);
        Shoot(target);
    }
}
