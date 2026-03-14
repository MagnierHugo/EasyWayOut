using UnityEngine;
using static SelectShotgunRoundSystem;

public abstract class Gun : MonoBehaviour, IShoot
{
    public abstract void Shoot(IShootable target);

    protected Mag mag = new Mag();

    public Mag GetMag() => mag;

    public TransformData idleTransformData;
    public TransformData pointedAtOpponentTransformData;
    public TransformData pointedAtSelfTransformData;
}
