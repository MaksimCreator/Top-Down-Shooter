using UnityEngine;

public class Shotgun : Weapon
{
    private float _angelBullet;

    public Shotgun(Transform weapon,float angelBullet, float bulletDistanse, int damage, float bulletPerSecond, int numberBullet) : base(weapon,bulletDistanse, damage, bulletPerSecond, numberBullet)
    {
        _angelBullet = angelBullet;
    }

    protected override Bullet GetBullet()
    => new ShotgunBullet(_angelBullet, Damage, BulletDistanse);
}
