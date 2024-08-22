using UnityEngine;

public abstract class Bullet : IBulletDate
{
    private const int Speed = 50;

    public readonly int Damage;

    private IIsFsmState _fsm;
    private Vector3 _direction;

    protected Transform BulletTransform { get; private set; }
    protected Vector3 StartPosition { get; private set; }

    public Vector3 Direction { get; private set; }
    public bool IsEnd { get; protected set; }
    public bool IsMove => IsEnd == false;

    public Bullet(int damage,Fsm fsm)
    {
        Damage = damage;
        _fsm = fsm;
    }

    public void InitDirection(Vector3 direction)
    => _direction = direction;

    public void StartMovemeng(GameObject bullet)
    {
        BulletTransform = bullet.transform;
        StartPosition = bullet.transform.position;
        IsEnd = false;
    }

    public void Update(float delta)
    {
        if (_fsm.IsState<FsmStateBulletMovemeng>() == false)
            return;

        Vector3 direction = BulletTransform.TransformDirection(BulletTransform.forward) * Speed * delta;
        Direction = GetDirection(direction);
    }

    public virtual void OnEnd()
    => IsEnd = true;

    protected virtual Vector3 GetDirection(Vector3 deltaDirection)
    => deltaDirection;
}
