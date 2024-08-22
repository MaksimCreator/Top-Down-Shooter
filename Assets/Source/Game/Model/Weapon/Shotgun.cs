using UnityEngine;

public class Shotgun : Weapon
{
    private readonly Fsm _fsm;
    private readonly float _angelBullet;
    private readonly float _bulletDistance;

    public Shotgun(Fsm fsm,Transform weapon,float angelBullet, float bulletDistance, int damage, float bulletPerSecond, int numberBullet) : base(weapon, damage, bulletPerSecond, numberBullet)
    {
        _fsm = fsm;
        _angelBullet = angelBullet;
        _bulletDistance = bulletDistance;
    }

    protected override Bullet GetBullet(Vector3 targetPosition)
    => new ShotgunBullet(_fsm, _angelBullet, Damage, _bulletDistance);
}
