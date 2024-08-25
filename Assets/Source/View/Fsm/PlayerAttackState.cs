using Model;
using UniRx;
using UnityEngine;

public class PlayerAttackState : FsmState
{
    private readonly BulletController _controller;
    private readonly StateData _date;

    private CompositeDisposable _disposable;

    public PlayerAttackState(BulletController controller,ServiceLocator locator,Fsm fsm) : base(fsm)
    {
        _controller = controller;
        _date = new StateData(locator.GetSevice<InputRouter>(),locator.GetSevice<MouseDate>());
    }

    public override void Enter()
    {
        _controller.Enable();
        _disposable = Timers.StartInfiniteTimer(1 / _controller.BulletPerSecond, () => _controller.Shoot(_date.Direction));
    }

    public override void Update()
    {
        if (_date.IsAttack == false)
            Fsm.SetState<PlayerIdelState>();
    }

    public override void Exit()
    {
        _controller.Disable();
        _disposable.Clear();
    }

    private class StateData
    {
        private readonly IInputService _input;
        private readonly IMousePosition _cursor;

        public bool IsAttack => _input.IsPerfomedAttack();
        public Vector3 Direction => _cursor.mouseToWorldPosition;

        public StateData(IInputService service, IMousePosition cursor)
        {
            _input = service;
            _cursor = cursor;
        }
    }
}