using UnityEngine;

public class FsmStateMovemeng : FsmState
{
    private readonly IDirection _move;
    protected Transform Entity { private get; set; }

    public FsmStateMovemeng(Transform entity,IDirection direction,Fsm fsm) : base(fsm)
    {
        _move = direction;
        Entity = entity;
    }

    public override void Update()
    => Entity.Translate(_move.Direction,Space.Self);
}
