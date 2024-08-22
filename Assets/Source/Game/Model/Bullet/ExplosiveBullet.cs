using UnityEngine;

public class ExplosiveBullet : Bullet
{
    private readonly float _radius;
    private readonly Vector3 _endPosition;

    public ExplosiveBullet(Fsm fsm,float radius,int damage, Vector3 endPosition) : base(damage,fsm)
    {
        _radius = radius;
        _endPosition = endPosition;
    }

    public override void OnEnd()
    {
        Collider[] targets = Physics.OverlapSphere(BulletTransform.position, _radius);

        foreach (Collider colider in targets)
        {
            if (colider.gameObject.TryGetComponent(out PhysicsEventBroadcaster broadcaster) && broadcaster.TryGetComponent(out Enemy enemy))
                enemy.TakeDamage(Damage);
        }

        base.OnEnd();
    }

    protected override Vector3 GetDirection(Vector3 deltaDirection)
    {
        float lengch = (BulletTransform.position + deltaDirection).magnitude;

        if (_endPosition.magnitude <= lengch)
        {
            IsEnd = true;
            return _endPosition - BulletTransform.position;
        }

        return deltaDirection;
    }
}
