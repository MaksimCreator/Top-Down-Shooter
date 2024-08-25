using UnityEngine;

public class PlayerMovemengState : FsmState
{
    private readonly IDirection _move;
    private readonly IRotate _rotate;
    private readonly StateDate _date;

    protected Transform Entity { private get; set; }

    public PlayerMovemengState(Transform entity,IDirection direction,IInputService service,IRotate rotate,Fsm fsm) : base(fsm)
    {
        _move = direction;
        _rotate = rotate;
        Entity = entity;

        _date = new StateDate(service);
    }

    public override void Update()
    {
        if ((_rotate.IsRotate || _date.IsMove) == false)
            Fsm.SetState<PlayerIdelState>();

        Entity.rotation = Quaternion.RotateTowards(Entity.rotation, _rotate.Rotation, _rotate.MaxDegreesDelta);
        Entity.Translate(_move.Direction, Space.Self);
    }

    private class StateDate
    {
        private readonly IInputService _input;

        public bool IsMove => _input.IsPerfomedMovemeng();

        public StateDate(IInputService input)
        {
            _input = input;
        }
    }
}