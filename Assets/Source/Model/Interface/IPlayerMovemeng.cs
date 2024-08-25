using UnityEngine;

public interface IPlayerMovemeng : IDeltaUpdatable
{
    void Move(float x, float y, float delta);
    void Rotate(Vector3 target);
}