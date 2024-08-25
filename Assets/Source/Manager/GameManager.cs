using State;
using Model;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<PhysicsEventBroadcaster> _walls = new();
        [SerializeField] private PhysicsEventBroadcaster _player;
        [SerializeField] private PlayerConffig _playerConffig;
        [SerializeField] private MapConffig _mapConffig;
        [SerializeField] private BonusSpawnerConffig _bonusSpawnerConffig;
        [SerializeField] private EnemySpawnreConffig _enemySpawnreConffig;
        [SerializeField] private AllWeaponGameConffig _allWeaponConffig;
        [SerializeField] private AllGainConffig _allGainConffig;
        [SerializeField] private AllSoldierConffig _allSoliderConffig;
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private BulletViewFactory _bulletFactory; 
        [SerializeField] private EnemyViewFactory _enemyFactory;
        [SerializeField] private GainViewFactory _gainFactory;
        [SerializeField] private WeaponViewFactory _weaponFactory;
        [SerializeField] private ZoneViewFactory _zoneFactory;
        [SerializeField] private EndGameView _endGameView;
        [SerializeField] private ExitMenuView _exitMenuView;
        [SerializeField] private GamplayMenuView _gamplayMenuView;

        private readonly Fsm _fsmGame;

        private SpawnerBonus _spawnerBonus;

        private bool _isInachelisate = false;

        public void ServiceLocatorInachelisate(Camera camera,ServiceLocator locator, Transform map)
        {
            MouseDate date = new MouseDate(camera);
            MapBounds bounds = new MapBounds(map, _mapConffig.MinDistanceSpawnZone);

            ServiceInachelisite.RegisterService(date, _bulletFactory, _enemyFactory, _gainFactory, _weaponFactory,
                _zoneFactory, locator, bounds, _allGainConffig, _allSoliderConffig, _allWeaponConffig,
                _playerConffig, _enemySpawnreConffig, _mapConffig, _bonusSpawnerConffig);
        }

        public void Activate(Inventary inventary,Wallet wallet, PlayerHealth health,SpawnerBonus spawner,BulletSimulation bulletSimulation,Transform transformWeapon, ServiceLocator locator)
        {
            CollisionRecords records = new CollisionRecords(_gainFactory, _weaponFactory, bulletSimulation, inventary, spawner, transformWeapon);
            ServiceInachelisite.RegisterPhysicsRouter(locator,records);

            _fsmGame.BindState(new LevelInachelisateState(locator, _fsmGame))
                .BindState(new GuiInatelisateState(wallet, health, locator.GetSevice<PlayerDate>(), this, _gamplayMenuView, _exitMenuView, _endGameView, _fsmGame))
                .BindState(new PhysicsRoutingState(this, _walls, _player, locator.GetSevice<PhysicsRouter>(), _fsmGame));

            _fsmGame.SetState<LevelInachelisateState>();
            spawner.Enable();
            enabled = true;
            _isInachelisate = true;
        }

        public void Enable()
        {
            if (_isInachelisate == false)
                throw new InvalidOperationException();

            _playerManager.Enable();
            _enemyManager.Enable();
            _spawnerBonus.StartTimer();
        }

        public void Disable()
        {
            _playerManager.Disable();
            _enemyManager.Disable();
            _spawnerBonus.StopTimer();
        }

        public void AllStop()
        {
            _playerManager.AllStop();
            _enemyManager.AllStop();
            _spawnerBonus.Disable();
            _fsmGame.SetState<IdelState>();
            _isInachelisate = false;
        }
    }
}