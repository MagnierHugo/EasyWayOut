using UnityEngine;

public abstract class Gun : MonoBehaviour, IShoot
{
    public abstract void Shoot(IShootable target);
}
