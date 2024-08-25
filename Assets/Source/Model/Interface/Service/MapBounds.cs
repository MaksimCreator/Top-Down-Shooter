using UnityEngine;

public class MapBounds : IMapBoundsService
{
    private readonly Transform _map;
    private readonly int _minDistance;

    public MapBounds(Transform mapBounds, int minDistance)
    {
        _map = mapBounds;
        _minDistance = minDistance;
    }

    public Vector3 GenerateRandomPositionWithinBounds(Generate type = Generate.Item)
    {
        Vector3 min = _map.position - _map.localScale / 2;
        Vector3 max = _map.position + _map.localScale / 2;

        if (type == Generate.Zone)
        {
            min.x += _minDistance * 2;
            min.z += _minDistance * 2;
            max.x -= _minDistance * 2;
            max.z -= _minDistance * 2;
        }

        return new Vector3(Random.Range(min.x, max.x), max.y, Random.Range(min.z, max.z));
    }

    public Vector3 GetCenterMap()
    {
        Vector3 mapPosition = _map.position;
        mapPosition.y += _map.localScale.y / 2;
        return mapPosition;
    }

    public enum Generate
    { 
        Zone,
        Item
    }
}