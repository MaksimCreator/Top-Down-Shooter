using UnityEngine;

[CreateAssetMenu(menuName = "Conffig/EnemySpawnre")]
public class EnemySpawnreConffig : ScriptableObject
{
    [SerializeField] private float _cooldownUpdateSpawnEnemy;
    [SerializeField] private float _deltaTimeDelay;
    [SerializeField] private float _startCooldownSpawnEnemy;
    [SerializeField] private float _minCooldownSpawn;

    public float CooldownUpdateSpawnEnemy => _cooldownUpdateSpawnEnemy;
    public float DeltaTiemDelay => _deltaTimeDelay;
    public float StartCooldownSpawnEnemy => _startCooldownSpawnEnemy;
    public float MinCooldownSpawn => _minCooldownSpawn;
}