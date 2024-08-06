using UnityEngine;

[CreateAssetMenu(menuName = "Conffig/Enemy")]
public class EnemyConffig : ScriptableObject
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _speed;

    public int MaxHealth => _maxHealth;
    public float Speed => _speed;
}