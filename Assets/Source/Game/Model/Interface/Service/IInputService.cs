using UnityEngine;

public interface IInputService : IService
{
    Vector2 Direction { get; }

    bool IsPerfomedMovemeng();
    bool IsPerfomedAttack();
}