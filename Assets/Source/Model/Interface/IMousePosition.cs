using UnityEngine;

public interface IMousePosition : IService
{
    Vector3 mouseToWorldPosition { get; }
}