using UnityEngine;

public abstract class Gun : MonoBehaviour, IShoot
{
    public abstract void Shoot(IShootable target);

    protected Mag mag = new Mag();

    public Mag GetMag() => mag;
}
