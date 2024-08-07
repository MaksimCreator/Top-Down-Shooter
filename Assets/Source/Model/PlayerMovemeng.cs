using System;
using UnityEngine;

public class PlayerMovemeng : IDirection,ITargetPosition
{
    private readonly ISpeed _playerSpeed;
    private readonly IPosition _playerPosition;
    private readonly Fsm _playerRotate = new();

    private Vector2 _direction;
    private float _deltaTime;
    
    public Vector3 Direction
    {
        get
        {
            Vector2 direction = _direction;
            _direction = Vector2.zero;
            return direction;
        }
    }
    public Vector3 TargetPosition { get; private set; }

    public PlayerMovemeng(float rotationPerSecond,Player player,Transform transformPlayer)
    {
        _playerSpeed = player;
        _playerPosition = player;

        _playerRotate
            .BindState(new FsmStateIdel(_playerRotate))
            .BindState(new FsmStateRotate(this,GetDeltaTime,transformPlayer,_playerRotate,rotationPerSecond));
    }

    public void Move(float x,float y,float delta)
    {
        if (delta <= 0)
            throw new InvalidOperationException(nameof(delta));

        if(x != 0)
            _direction.x = _direction.x * x * _playerSpeed.Speed * delta;

        if(y != 0)
            _direction.y = _direction.y * y * _playerSpeed.Speed * delta;
    }

    public void Update(float delta)
    { 
        _deltaTime = delta;
        _playerRotate.Update();
    }

    public void Rotate(Vector3 target)
    {
        TargetPosition = target;
        _playerRotate.SetState<FsmStateRotate>();
    }

    private float GetDeltaTime()
    => _deltaTime;
}