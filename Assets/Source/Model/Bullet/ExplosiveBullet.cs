using UnityEngine;

public class ExplosiveBullet : Bullet
{
    private readonly float _radius;

    public ExplosiveBullet(float radius,int damage, float bulletDistance) : base(damage, bulletDistance)
    {
        _radius = radius;
    }

    public override void OnEnd(Vector3 positionBullet)
    {
        Collider[] targets = Physics.OverlapSphere(positionBullet, _radius);

        foreach (Collider colider in targets)
        {
            if (colider.gameObject.TryGetComponent(out EnemyHealthEventBroadcaster broadcaster))
                broadcaster.TakeDamage(Damage);
        }
    }
}