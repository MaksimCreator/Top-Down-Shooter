using UnityEngine;

public abstract class Entity : ISpeed,IPosition
{
    private Transform _transform;

    public Vector3 Position => _transform.position;
    public float Speed { get; protected set; }

    public Entity(float speed,Transform transform = null)
    {
        Speed = speed;
        _transform = transform;
    }

    protected void TryAddTransformEntity(Transform entity)
    {
        if (_transform == null)
            _transform = entity;
    }

}