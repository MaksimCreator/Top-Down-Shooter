using UnityEngine;

public abstract class Bullet
{
    public const int Speed = 100;

    public readonly int Damage;
    public readonly float BulletDistance;

    public Bullet(int damage,float bulletDistance)
    {
        Damage = damage;
        BulletDistance = bulletDistance;
    }

    public virtual void OnEnd(Vector3 positionBullet) { }
}