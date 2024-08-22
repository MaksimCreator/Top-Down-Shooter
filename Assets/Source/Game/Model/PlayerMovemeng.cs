using System;
using UnityEngine;

public class PlayerMovemeng : IDirection, IRotate, IPlayerMovemeng
{
    private readonly ISpeed _playerSpeed;
    private readonly IPosition _playerTransform;
    private readonly IUpdatebleFsm _fsmPlayerRotate;
    private readonly float _rotationPerSecond;

    private Vector2 _direction;
    private float _maxDegreesDelta;
    private bool _isRotate;

    public Vector3 Direction
    {
        get
        {
            Vector2 direction = _direction;
            _direction = Vector2.zero;
            return direction;
        }
    }
    public Quaternion Rotation { get; private set; }
    public float MaxDegreesDelta => _maxDegreesDelta;
    public bool IsRotate => _isRotate;

    public PlayerMovemeng(float rotationPerSecond,Player player,Transform transformPlayer,Fsm fsm)
    {
        _playerSpeed = player;
        _playerTransform = player;
        _fsmPlayerRotate = fsm;
        _rotationPerSecond = rotationPerSecond;
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
        _maxDegreesDelta = _rotationPerSecond * delta;
        _fsmPlayerRotate.Update();
    }

    public void Rotate(Vector3 target)
    {
        Vector3 direction = target - _playerTransform.Position;
        direction.y = 0;

        if (direction.magnitude <= 0)
            _isRotate = false; 
        else 
            _isRotate = true;

        Rotation = Quaternion.LookRotation(direction);
    }
}
