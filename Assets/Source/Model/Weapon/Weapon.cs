using System;
using UnityEngine;

public abstract class Weapon
{
    public readonly float BulletPerSecond;

    protected readonly Transform PositionWeapon;
    protected readonly int Damage;
    private readonly int _bulletCount;

    public event Action<Bullet,Transform,Vector3> onShoot;

    protected Weapon(Transform weapon, int damage, float bulletPerSecond, int numberBullet = 1)
    {
        PositionWeapon = weapon;
        Damage = damage;
        BulletPerSecond = bulletPerSecond;
        _bulletCount = numberBullet;
    }

    public void Shoot(Vector3 direction)
    {
        for (int i = 0; i < _bulletCount; i++)
            onShoot?.Invoke(GetBullet(direction),PositionWeapon,direction);
    }

    protected virtual Bullet GetBullet(Vector3 targetPosition)
    => new DefoltBullet(Damage);
}