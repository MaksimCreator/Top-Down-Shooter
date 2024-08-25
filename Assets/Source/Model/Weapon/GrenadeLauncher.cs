using UnityEngine;

public class GrenadeLauncher : Weapon
{
    private readonly int _explosionRadius;

    public GrenadeLauncher(int explositionRadius,Transform weapon, int damage, float bulletPerSecond, int numberBullet = 1) : base(weapon, damage, bulletPerSecond, numberBullet)
    {
        _explosionRadius = explositionRadius;
    }

    protected override Bullet GetBullet(Vector3 targetPosition)
    => new ExplosiveBullet(_explosionRadius,Damage,targetPosition);
}