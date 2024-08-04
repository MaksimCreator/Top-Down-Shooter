using UnityEngine;

public class GrenadeLauncher : Weapon
{
    public readonly int ExplosionRadius;

    public GrenadeLauncher(Transform weapon, float bulletDistanse, int damage, float bulletPerSecond, int numberBullet = 1) : base(weapon, bulletDistanse, damage, bulletPerSecond, numberBullet)
    {
    }

    protected override Bullet GetBullet()
    => new ExplosiveBullet(ExplosionRadius,Damage,BulletDistanse);
}