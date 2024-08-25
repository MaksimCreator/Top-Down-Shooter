using UnityEngine;
using System.Collections.Generic;

public class SpawnerZone
{
    private readonly ZoneViewFactory _zoneViewFactory;
    private readonly IMapBoundsService _mapBounds;
    private readonly int _minDistance;

    private List<Vector3> _generatedZones = new();

    public SpawnerZone(ZoneViewFactory zoneViewFactory, IMapBoundsService mapBounds, int minDistance)
    {
        _zoneViewFactory = zoneViewFactory;
        _mapBounds = mapBounds;
        _minDistance = minDistance;
    }

    public void GenerateZone(Zone zone)
    {
        while (true)
        {
            Vector3 randomPosition = _mapBounds.GenerateRandomPositionWithinBounds(MapBounds.Generate.Zone);

            if (CheckDistance(randomPosition))
            {
                _generatedZones.Add(randomPosition);
                _zoneViewFactory.Creat(zone, randomPosition,Quaternion.identity);
                break;
            }
        }
    }

    private bool CheckDistance(Vector3 position)
    {
        foreach (Vector3 zone in _generatedZones)
        {
            if (Vector3.Distance(position, zone) < _minDistance)
                return false;
        }

        return true;
    }
}