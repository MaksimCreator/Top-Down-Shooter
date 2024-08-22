using UnityEngine;

public interface IPlayerMovemeng : IUpdatebleModel
{
    void Move(float x, float y, float delta);
    void Rotate(Vector3 target);
}