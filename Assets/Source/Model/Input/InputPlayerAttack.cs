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
            _fsm.SetState<PlayerAttackState>();
        else
            _fsm.SetState<PlayerIdelState>();
    }
}