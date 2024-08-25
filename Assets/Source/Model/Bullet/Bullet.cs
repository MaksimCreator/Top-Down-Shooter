using UnityEngine;

public abstract class Bullet : IBulletData
{
    private const int Speed = 50;

    public readonly int Damage;

    public Transform BulletTransform { get; private set; }
    protected Vector3 StartPosition { get; private set; }

    public Vector3 Direction { get; private set; }
    public bool IsEnd { get; protected set; }
    public bool isMoveming => IsEnd == false;

    public Bullet(int damage)
    {
        Damage = damage;
    }

    public void StartMovemeng(GameObject bullet)
    {
        BulletTransform = bullet.transform;
        StartPosition = bullet.transform.position;
        IsEnd = false;
    }

    public void Update(float delta)
    {
        if (isMoveming == false)
            return;

        Vector3 direction = BulletTransform.TransformDirection(BulletTransform.forward) * Speed * delta;
        Direction = GetDirection(direction);
    }

    public virtual void OnEnd()
    => IsEnd = true;

    protected virtual Vector3 GetDirection(Vector3 deltaDirection)
    => deltaDirection;
}
