using UnityEngine;

public interface IBulletData : IDirection,IEnd,IMovemeng
{
    Transform BulletTransform { get; }
}
