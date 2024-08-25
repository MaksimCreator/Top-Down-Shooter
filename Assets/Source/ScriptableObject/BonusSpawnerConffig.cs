using UnityEngine;

[CreateAssetMenu(menuName = "Conffig/BonusSpawner")]
public class BonusSpawnerConffig : ScriptableObject,IService
{
    [SerializeField] private float _cooldownSpawnWeapon;
    [SerializeField] private float _cooldownSpawnGain;
    [SerializeField] private float _cooldownDestryBonus;

    public float CooldownSpawnWeapon => _cooldownSpawnWeapon;
    public float CooldownSpawnGain => _cooldownSpawnGain;
    public float CooldownDestryBonus => _cooldownDestryBonus;
}
