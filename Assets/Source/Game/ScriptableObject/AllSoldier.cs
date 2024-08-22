using UnityEngine;

[CreateAssetMenu(menuName = "Conffig/AllSoldier")]
public class AllSoldier : ScriptableObject
{
    [SerializeField] private EnemyConffig _armorSoldier;
    [SerializeField] private EnemyConffig _nimbleSoldier;
    [SerializeField] private EnemyConffig _privateSoldier;

    public EnemyConffig ArmoredSoldier => _armorSoldier;
    public EnemyConffig NimbleSoldier => _nimbleSoldier;
    public EnemyConffig PrivateSoldier => _privateSoldier;
}