using UnityEngine;

public class InatelisateLevelCompositRoot : CompositRoot
{
    [SerializeField] private ZoneViewFactory _zoneViewFactory;
    [SerializeField] private Transform _zoneTransfom;
    [SerializeField] private Transform _map;
    [SerializeField] private MapConffig _mapConffig;

    public MapBounds Map { get; private set; }

    public override void Compos()
    {
        Physics.IgnoreLayerCollision(Constant.EnemyLayer,Constant.EnemyLayer);
        Physics.IgnoreLayerCollision(Constant.EnemyLayer, Constant.WallInvisibleLayer);

        Map = new MapBounds(_map,_zoneTransfom,_mapConffig.MinDistanceSpawnZone);
        SpawnerZone spawner = new SpawnerZone(_zoneViewFactory,Map,_mapConffig.MinDistanceSpawnZone);

        CreatZones(spawner,new SlowZone(),_mapConffig.CountSlowZone);
        CreatZones(spawner, new DeathZone(), _mapConffig.CountDeathZone);
    }

    private void CreatZones(SpawnerZone spawner,Zone zone, int count)
    {
        for(int i = 0; i < count;i++)
            spawner.GenerateZone(zone);
    }
}