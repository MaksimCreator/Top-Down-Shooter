using UnityEngine;

public class PlayerConffig : ScriptableObject
{
    [SerializeField] private float _rotationPerSecond;
    [SerializeField] private float _speed;

    public float RotationPerSecond => _rotationPerSecond;
    public float Speed => _speed;
}