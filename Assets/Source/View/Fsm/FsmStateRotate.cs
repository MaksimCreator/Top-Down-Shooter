using System;
using UnityEngine;

public class FsmStateRotate: FsmState
{
    private readonly ITargetPosition _rotate;
    private readonly Func<float> _getDeltaTime;
    private readonly Transform _entity;
    private readonly float _rotationPerSecond;

    public FsmStateRotate(ITargetPosition rotate,Func<float> getDeltaTime,Transform entity, Fsm fsm, float rotationPerSecond) : base(fsm)
    {
        _rotate = rotate;
        _getDeltaTime = getDeltaTime;
        _entity = entity;
        _rotationPerSecond = rotationPerSecond;
    }

    public override void Update()
    {
        Vector3 direction = _rotate.TargetPosition - _entity.position;
        direction.y = 0;

        if (direction.magnitude <= 0)
            Fsm.SetState<FsmStateIdel>();

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _entity.rotation = Quaternion.RotateTowards(_entity.rotation, targetRotation, _rotationPerSecond * _getDeltaTime.Invoke());
    }
}