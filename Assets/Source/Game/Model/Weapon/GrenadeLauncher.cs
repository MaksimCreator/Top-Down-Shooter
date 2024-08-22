using UnityEngine;

public class GrenadeLauncher : Weapon
{
    private readonly Fsm _fsm;
    private readonly int _explosionRadius;

    public GrenadeLauncher(Fsm fsm,int explositionRadius,Transform weapon, int damage, float bulletPerSecond, int numberBullet = 1) : base(fsm, weapon, damage, bulletPerSecond, numberBullet)
    {
        _fsm = fsm;
        _explosionRadius = explositionRadius;
    }

    protected override Bullet GetBullet(Vector3 targetPosition)
    => new ExplosiveBullet(_fsm,_explosionRadius,Damage,targetPosition);
}