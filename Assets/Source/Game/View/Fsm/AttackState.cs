using Model;
using UniRx;
using UnityEngine;

public class AttackState : FsmState
{
    private readonly PlayerPresenter _presenter;
    private readonly StateDate _date;
    private CompositeDisposable _disposable;

    public AttackState(PlayerPresenter presenter,PlayerController controller,Fsm fsm,ServiceLocator locator) : base(fsm)
    {
        _presenter = presenter;
        _date = new StateDate(locator.GetSevice<IInputService>(),controller);
    }

    public override void Enter()
    {
        _presenter.Enable();
        _disposable = Timers.StartInfiniteTimer(1 / _presenter.BulletPerSecond, () => _presenter.Shoot(_date.Direction));
    }

    public override void Update()
    {
        if (_date.IsAttack == false)
            Fsm.SetState<IdelState>();
    }

    public override void Exit()
    {
        _presenter.Disable();
        _disposable.Clear();
    }

    private class StateDate
    {
        private readonly IInputService _input;
        private readonly ICursorDate _cursor;

        public bool IsAttack => _input.IsPerfomedAttack();
        public Vector3 Direction => _cursor.MousePosition;

        public StateDate(IInputService service, ICursorDate cursor)
        {
            _input = service;
            _cursor = cursor;
        }
    }
}