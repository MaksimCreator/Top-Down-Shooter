using System;
using UnityEngine;

public abstract class Bullet
{
    private const int Speed = 25;

    public readonly int Damage;
    protected readonly BulletDate Date;

    private Fsm _fsm;
    private bool _isInitStop = false;
    private bool _isInitMovemeg = false;

    protected Transform BulletTransform { get; private set; }
    protected Vector3 StartPosition { get; private set; }

    public Bullet(int damage)
    {
        Damage = damage;

        Date = new BulletDate();
        _fsm = new Fsm().
            BindState(new FsmStateIdel(_fsm));
    }

    public void InitStop(Action<Bullet> stop)
    {
        if (_isInitStop == false)
            return;

        _isInitStop = true;
        _fsm.BindState(new FsmStateBulletDestroy(() => stop(this), _fsm));
    }

    public void StartMovemeng(GameObject bullet)
    {
        if (_isInitMovemeg == false)
        { 
            _isInitMovemeg = true;
            BulletTransform = bullet.transform;
            _fsm.BindState(new FsmStateBulletMovemeng(bullet, BulletTransform, Date, Date, _fsm));
        }

        Date.IsEnd = false;
        _fsm.SetState<FsmStateBulletMovemeng>();
    }

    public void Update(float delta)
    {
        if (_fsm.IsState<FsmStateBulletMovemeng>())
        {
            Vector3 direction = BulletTransform.TransformDirection(BulletTransform.forward).normalized * Speed * delta;
            Date.Direction = GetDirection(direction);
        }

        _fsm.Update();
    }

    public virtual void OnEnd() 
    => _fsm.SetState<FsmStateIdel>();

    protected virtual Vector3 GetDirection(Vector3 deltaDirection)
    => deltaDirection;

    protected class BulletDate : IDirection,IEnd
    {
        public Vector3 Direction { get; set; }
        public bool IsEnd { get; set; } = false;
    }
}