using System;
using UnityEngine;

public abstract class Weapon
{
    public readonly float BulletPerSecond;

    protected readonly Transform PositionWeapon;
    protected readonly int Damage;
    protected readonly float BulletDistanse;

    private readonly int _bulletCount;

    public event Action<Bullet,Vector3,Vector3> onShoot;

    protected Weapon(Transform weapon, float BulletDistanse, int damage, float bulletPerSecond, int numberBullet = 1)
    {
        PositionWeapon = weapon;
        Damage = damage;
        BulletPerSecond = bulletPerSecond;
        _bulletCount = numberBullet;
    }

    public void Shoot(Vector3 mouseWorldPosition)
    {
        for (int i = 0; i < _bulletCount; i++)
            onShoot?.Invoke(GetBullet(),PositionWeapon.position,(mouseWorldPosition - PositionWeapon.position).normalized);
    }

    protected virtual Bullet GetBullet()
    => new DefoltBullet(Damage,BulletDistanse);
}