public abstract class FsmState
{
    protected Fsm Fsm;

    protected FsmState(Fsm fsm)
    {
        Fsm = fsm;
    }

    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void Exit() { }
}