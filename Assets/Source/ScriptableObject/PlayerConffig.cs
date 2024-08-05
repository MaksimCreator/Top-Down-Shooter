using UnityEngine;

[CreateAssetMenu(menuName = "Conffig/Player")]
public class PlayerConffig : ScriptableObject
{
    [SerializeField] private float _rotationPerSecond;
    [SerializeField] private float _speed;
    [SerializeField] private int _health;

    public float RotationPerSecond => _rotationPerSecond;
    public float Speed => _speed;
    public int Health => _health;
}