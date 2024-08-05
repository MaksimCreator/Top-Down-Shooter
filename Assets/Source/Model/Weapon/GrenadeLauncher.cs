using UnityEngine;

public class GrenadeLauncher : Weapon
{
    private readonly int _explosionRadius;

    public GrenadeLauncher(Transform weapon, int damage, float bulletPerSecond, int numberBullet = 1) : base(weapon, damage, bulletPerSecond, numberBullet)
    {
    }

    protected override Bullet GetBullet()
    => new ExplosiveBullet(_explosionRadius,Damage,Input.mousePosition);
}