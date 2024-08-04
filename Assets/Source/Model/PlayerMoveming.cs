using System;
using UnityEngine;

public class PlayerMoveming : IDirection
{
    private readonly float _rotationPerSecond;
    private readonly ISpeed _player; 
    private readonly Fsm _playerRotate;

    private Vector2 _direction;
    
    public Vector2 Direction
    {
        get
        {
            Vector2 direction = _direction;
            _direction = Vector2.zero;
            return direction;
        }
    }
    public Vector2 Rotation { get; }

    public PlayerMoveming(float rotationPerSecond,Fsm playerRotate,Player player)
    {
        _rotationPerSecond = rotationPerSecond;
        _playerRotate = playerRotate;
        _player = player;
    }

    public void Move(float x,float y,float delta)
    {
        if (delta <= 0)
            throw new InvalidOperationException(nameof(delta));

        if(x != 0)
            _direction.x = _direction.x * x * _player.Speed * delta;

        if(y != 0)
            _direction.y = _direction.y * y * _player.Speed * delta;

        Rotate(_direction.y);
    }

    public void Update(float delta)
    {

    }

    public void Rotate(float rotation)
    {

    }
}
public class FsmStateMovemeng : FsmState
{
    private readonly IDirection _direction;

    public FsmStateMovemeng(IDirection direction,Fsm fsm) : base(fsm)
    {
        _direction = direction;
    }
}
public class FsmStateRotate : FsmState
{
    private readonly IRotation _rotate;

    public FsmStateRotate(IRotation rotate,Fsm fsm) : base(fsm)
    {
        _rotate = rotate;
    }
}
public class FsmStateIdel : FsmState
{
    public FsmStateIdel(Fsm fsm) : base(fsm)
    {
    }
}