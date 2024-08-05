public class InputPlayerAttack
{
    private readonly Fsm _fsm;

    public InputPlayerAttack(Fsm fsm)
    {
        _fsm = fsm;
    }

    public void Update(bool isAttack)
    {
        if (isAttack)
            _fsm.SetState<FsmStateAttack>();
        else
            _fsm.SetState<FsmStateIdel>();
    }
}