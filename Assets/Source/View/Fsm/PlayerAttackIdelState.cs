namespace State
{
    public class PlayerAttackIdelState : FsmState
    {
        private readonly InputData _inputService;

        public PlayerAttackIdelState(IInputService inputService,Fsm fsm) : base(fsm)
        {
            _inputService = new InputData(inputService);
        }

        public override void Update()
        {
            if (_inputService.IsAttack)
                Fsm.SetState<PlayerAttackState>();
        }

        private class InputData
        {
            private readonly IInputService _inputService;

            public bool IsAttack => _inputService.IsPerfomedAttack();

            public InputData(IInputService inputService)
            {
                _inputService = inputService;
            }
        }
    }
}
