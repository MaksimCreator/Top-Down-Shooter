using UnityEngine;

public interface IEnemyData : IDirection, IMovemeng
{
    Transform EnemyTransform { get; }
    Vector3 TargetPosition { get; }
}