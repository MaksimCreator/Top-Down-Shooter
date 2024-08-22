using UnityEngine;
using System;

public abstract class Enemy : Entity,IDirection,IUpdatebleModel
{
    private readonly Fsm _fsm = new();
    
    private IPosition _target;
    private Vector3 _direction;
    private bool _isInitMovemeng;

    public EnemyHealth Health { get; private set; }
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
        _fsm.BindState(new IdelState(_fsm));
        _fsm.SetState<IdelState>();
    }

    public void EnterStateIdel()
    => _fsm.SetState<IdelState>();

    public void StartMovemeng(Player player,Transform transform)
    {
        if (_isInitMovemeng == false)
        {
            TryAddTransformEntity(transform);
            _fsm.BindState(new FsmStateEnemyMovemeng(_fsm,player,this,transform));
            _isInitMovemeng = true;
        }

        _target = player;
        _fsm.SetState<FsmStateEnemyMovemeng>();
    }

    public void Update(float delta)
    {
        if (delta <= 0)
            throw new InvalidOperationException();

        Vector3 way = _target.Position - Position;
        Vector3 direction = way.normalized * Speed * delta;

        _direction += direction;
        _fsm.Update();
    }

    public void BindHealth(EnemyHealth health)
    => Health = health;

    public void TakeDamage(int damage)
    => Health.TakeDamage(damage);
}