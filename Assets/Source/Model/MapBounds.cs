using UnityEngine;

public class MapBounds
{
    private readonly Transform _mapBounds;
    private readonly Transform _transfomPosition;
    private readonly int _minDistance;

    public MapBounds(Transform mapBounds,Transform transform, int minDistance)
    {
        _mapBounds = mapBounds;
        _minDistance = minDistance;
        _transfomPosition = transform;
    }

    public Transform GenerateRandomPositionWithinBounds(Generate type = Generate.Item)
    {
        Vector3 min = _mapBounds.position - _mapBounds.localScale / 2;
        Vector3 max = _mapBounds.position + _mapBounds.localScale / 2;

        if (type == Generate.Zone)
        {
            min.x += _minDistance / 2;
            min.z += _minDistance / 2;
            max.x -= _minDistance / 2;
            max.z -= _minDistance / 2;
        }

        Vector3 position = new Vector3(Random.Range(min.x, max.x), max.y, Random.Range(min.z, max.z));
        _transfomPosition.position = position;
        return _transfomPosition;
    }

    public enum Generate
    { 
        Zone,
        Item
    }
}