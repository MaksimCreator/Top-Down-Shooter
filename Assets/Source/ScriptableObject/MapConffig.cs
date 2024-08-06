using UnityEngine;

public class MapConffig : ScriptableObject
{
    [SerializeField] private int _minDistanceSpawnZone;
    [SerializeField] private int _countSlowZone;
    [SerializeField] private int _countDeathZone;

    public int MinDistanceSpawnZone => _minDistanceSpawnZone;
    public int CountSlowZone => _countSlowZone;
    public int CountDeathZone => _countDeathZone;
}