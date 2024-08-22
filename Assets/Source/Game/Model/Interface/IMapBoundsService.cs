using UnityEngine;
using static MapBounds;

public interface IMapBoundsService : IService
{
    Vector3 GenerateRandomPositionWithinBounds(Generate type = Generate.Item);
    Vector3 GetCenterMap();
}