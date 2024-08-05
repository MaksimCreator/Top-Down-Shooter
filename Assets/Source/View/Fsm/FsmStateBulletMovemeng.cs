using UnityEngine;

public class FsmStateBulletMovemeng : FsmStateMovemeng
{
    private readonly IEnd _end;
    private GameObject _bullet;

    public FsmStateBulletMovemeng(GameObject bullet,Transform entity,IEnd end, IDirection direction, Fsm fsm) : base(entity, direction, fsm)
    {
        _end = end;
    }

    public override void Update()
    {
        base.Update();

        if (_end.IsEnd)
            Fsm.SetState<FsmStateBulletDestroy>();
    }
}
