using UnityEngine;

public class BulletMovemengState : FsmState
{
    private readonly IEnd _end;
    private readonly IDirection _direction;
    private readonly Transform _transfom;

    public BulletMovemengState(Transform entity,IEnd end, IDirection direction, Fsm fsm) : base(fsm)
    {
        _end = end;
        _transfom = entity;
    }

    public override void Update()
    {
        _transfom.Translate(_direction.Direction);

        if (_end.IsEnd)
            Fsm.SetState<BulletDestroyState>();
    }
}
