using UnityEngine;

public class ShotgunBullet : Bullet
{
    public readonly float AngelBullet;

    private float _bulletDistance;

    public ShotgunBullet(float angelBullet,int damage, float bulletDistance) : base(damage)
    {
        AngelBullet = angelBullet;
        _bulletDistance = bulletDistance;
    }

    protected override Vector3 GetDirection(Vector3 deltaDirection)
    {
        float lengch = StartPosition.magnitude - (BulletTransform.position + deltaDirection).magnitude;

        if (lengch >= _bulletDistance)
        {
            Date.IsEnd = true;
            return BulletTransform.position - deltaDirection.normalized * _bulletDistance + StartPosition;
        }

        return deltaDirection;
    }
}