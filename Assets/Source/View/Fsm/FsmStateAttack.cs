using System;

public class FsmStateAttack : FsmState
{
    private readonly Inventary _inventary;
    private IDisposable _disposable;

    public FsmStateAttack(Inventary inventary,Fsm fsm) : base(fsm)
    {
        _inventary = inventary;
    }

    public override void Enter()
    {
        _inventary.Weapon.Shoot();
        _disposable = Timer.StartInfiniteTimer(1 / _inventary.Weapon.BulletPerSecond, () => _inventary.Weapon.Shoot());
    }

    public override void Exit()
    => _disposable.Dispose();
}