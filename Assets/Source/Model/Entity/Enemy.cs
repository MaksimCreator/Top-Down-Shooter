using UnityEngine;
using System;

public abstract class Enemy : Entity,IDeltaUpdatable,IEnemyData
{
    private IPosition _target;
    private Vector3 _direction;

    public EnemyHealth Health { get; private set; }
    public Transform EnemyTransform { get; private set; }
    public bool isMoveming { get; set; }
    public Vector3 TargetPosition => _target.Position;
    public Vector3 Direction 
    {   get 
        {
            Vector3 direction = _direction;
            _direction = Vector3.zero;
            return direction;
        } 
    }

    public Enemy(float speed) : base(speed)
    {

    }

    public void StartMovemeng(Player player,Transform transform) 
    {
        TryAddTransformEntity(transform);
        EnemyTransform = transform;
        _target = player;
        isMoveming = true;
    }

    public void Update(float delta)
    {
        if (delta <= 0)
            throw new InvalidOperationException();

        Vector3 way = _target.Position - Position;
        Vector3 direction = way.normalized * Speed * delta;

        _direction += direction;
    }

    public void BindHealth(EnemyHealth health)
    => Health = health;

    public void TakeDamage(int damage)
    => Health.TakeDamage(damage);
}