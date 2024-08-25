using System;
using View;

public class BulletDestroyState : FsmState
{
    private readonly Action _destroy;

    public BulletDestroyState(Action destroy,Fsm fsm) : base(fsm)
    {
        _destroy = destroy;
    }

    public override void Enter()
    { 
        _destroy.Invoke();
        Fsm.SetState<BulletIdelState>();
    }
}