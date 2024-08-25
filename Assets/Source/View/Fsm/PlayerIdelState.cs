public class PlayerIdelState : FsmState
{
    private readonly PlayerMovemengInput _inputService;

    public PlayerIdelState(IInputService service,Fsm fsm) : base(fsm)
    {
        _inputService = new PlayerMovemengInput(service);
    }

    public override void Update()
    {
        if (_inputService.IsAttack || _inputService.IsMoveming)
            Fsm.SetState<PlayerMovemengState>();
    }

    private class PlayerMovemengInput
    {
        private readonly IInputService _input;

        public bool IsMoveming => _input.IsPerfomedMovemeng();
        public bool IsAttack => _input.IsPerfomedAttack();

        public PlayerMovemengInput(IInputService service)
        {
            _input = service;
        }
    }
}