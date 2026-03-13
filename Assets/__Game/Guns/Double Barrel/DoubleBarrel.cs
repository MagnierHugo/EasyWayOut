using UnityEngine;

public class DoubleBarrel : Gun, IHaveSpecial
{
    [SerializeField] private MagManager[] mags = new MagManager[2];
    private int currentMag = 0;
    private IShootable lastSwitch;

    public override void Shoot(IShootable target)
    {
        bool bullet = mags[currentMag].GetBullet();
        if (bullet) target.GetShot();
        else target.EmptyShot();
    }

    public void Special(IShootable target)
    {
        if (lastSwitch == target) return;

        currentMag = currentMag == 0 ? 1 : 0;
        lastSwitch = target;
    }
}
