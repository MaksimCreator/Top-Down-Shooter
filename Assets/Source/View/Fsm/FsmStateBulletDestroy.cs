using System;

public class FsmStateBulletDestroy : FsmState
{
    private readonly Action _destroy;

    public FsmStateBulletDestroy(Action destroy,Fsm fsm) : base(fsm)
    {
        _destroy = destroy;
    }

    public override void Enter()
    => _destroy.Invoke();
}