using UnityEngine;
using System;

public abstract class Enemy : Entity,IDirection
{
    private readonly Fsm _fsm = new();
    private readonly IPosition _target;
    
    private EnemyHealth _health;
    private Vector3 _direction;
    private bool _isInitMovemeng;

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
        _fsm
            .BindState(new FsmStateIdel(_fsm));
    }

    public void EnterStateIdel()
    => _fsm.SetState<FsmStateIdel>();

    public void StartMovemeng(Player player,Transform transform)
    {
        if (_isInitMovemeng == false)
        {
            TryAddTransformEntity(transform);
            _fsm.BindState(new FsmStateEnemyMovemeng(_fsm,player,this,transform));
            _isInitMovemeng = true;
        }

        _fsm.SetState<FsmStateMovemeng>();
    }

    public void Update(float delta)
    {
        if (delta <= 0)
            throw new InvalidOperationException();

        Vector3 way = Position - _target.Position;
        Vector3 direction = way / Speed * delta;
        direction.x /= way.x;
        direction.y /= way.y;
        direction.z /= way.z;

        _fsm.Update();
    }

    public void BindHealth(EnemyHealth health)
    => _health = health;

    public void TakeDamage(int damage)
    => _health.TakeDamage(damage);
}