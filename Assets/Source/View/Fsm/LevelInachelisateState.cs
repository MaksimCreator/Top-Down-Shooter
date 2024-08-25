using Model;
using System;
using UnityEngine;

namespace State
{
    public class LevelInachelisateState : FsmState
    {
        private readonly ZoneViewFactory _zoneViewFactory;
        private readonly MapConffig _mapConffig;
        private readonly IMapBoundsService _mapBounds;

        public LevelInachelisateState(ServiceLocator locator,Fsm fsm) : base(fsm)
        {
            _zoneViewFactory = locator.GetSevice<ZoneViewFactory>();
            _mapBounds = locator.GetSevice<MapBounds>();
            _mapConffig = locator.GetSevice<MapConffig>();
        }

        public override void Enter()
        {
            Physics.IgnoreLayerCollision(Constant.EnemyLayer, Constant.EnemyLayer);
            Physics.IgnoreLayerCollision(Constant.EnemyLayer, Constant.WallInvisibleLayer);
            Physics.IgnoreLayerCollision(Constant.BulletLayer, Constant.WeaponLayer);

            SpawnerZone spawner = new SpawnerZone(_zoneViewFactory, _mapBounds, _mapConffig.MinDistanceSpawnZone);

            CreatZones(spawner, CreatSlowZone, _mapConffig.CountSlowZone);
            CreatZones(spawner, CreatDeathZone, _mapConffig.CountDeathZone);

            Fsm.SetState<GuiInatelisateState>();
        }

        private SlowZone CreatSlowZone()
        => new SlowZone();

        private DeathZone CreatDeathZone()
        => new DeathZone();

        private void CreatZones(SpawnerZone spawner, Func<Zone> creat, int count)
        {
            for (int i = 0; i < count; i++)
                spawner.GenerateZone(creat.Invoke());
        }
    }
}
