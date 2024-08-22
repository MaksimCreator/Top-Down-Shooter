using UnityEngine;

public class FsmStateEnemyMovemeng : FsmState
{
    private readonly IPosition _target;
    private readonly IDirection _direction;
    private readonly Transform _transform;

    public FsmStateEnemyMovemeng(Fsm fsm,IPosition position,IDirection direction,Transform enemy) : base(fsm)
    {
        _target = position;
        _direction = direction;
        _transform = enemy;
    }

    public override void Update()
    {
        _transform.Translate(_direction.Direction, Space.World);
        _transform.LookAt(_target.Position);
    }
}