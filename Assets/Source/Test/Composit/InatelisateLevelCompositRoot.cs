using System;
using UnityEngine;

public class InatelisateLevelCompositRoot : CompositRoot
{
    [SerializeField] private ZoneViewFactory _zoneViewFactory;
    [SerializeField] private Transform _map;
    [SerializeField] private MapConffig _mapConffig;

    public MapBounds MapBounds { get; private set; }

    public override void Compos()
    {
        Physics.IgnoreLayerCollision(Constant.EnemyLayer,Constant.EnemyLayer);
        Physics.IgnoreLayerCollision(Constant.EnemyLayer, Constant.WallInvisibleLayer);
        Physics.IgnoreLayerCollision(Constant.BulletLayer, Constant.PlayerLayer);

        MapBounds = new MapBounds(_map,_mapConffig.MinDistanceSpawnZone);
        SpawnerZone spawner = new SpawnerZone(_zoneViewFactory,MapBounds,_mapConffig.MinDistanceSpawnZone);

        CreatZones(spawner,CreatSlowZone, _mapConffig.CountSlowZone);
        CreatZones(spawner,CreatDeathZone, _mapConffig.CountDeathZone);
    }

    private SlowZone CreatSlowZone()
    => new SlowZone();

    private DeathZone CreatDeathZone()
    => new DeathZone();

    private void CreatZones(SpawnerZone spawner,Func<Zone> creat, int count)
    {
        for (int i = 0; i < count; i++)
            spawner.GenerateZone(creat.Invoke());
    }
}