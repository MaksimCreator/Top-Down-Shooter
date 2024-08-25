using UnityEngine;

[CreateAssetMenu(menuName = "Conffig/Map")]
public class MapConffig : ScriptableObject, IService
{
    [SerializeField] private int _minDistanceSpawnZone;
    [SerializeField] private int _countSlowZone;
    [SerializeField] private int _countDeathZone;

    public int MinDistanceSpawnZone => _minDistanceSpawnZone;
    public int CountSlowZone => _countSlowZone;
    public int CountDeathZone => _countDeathZone;
}