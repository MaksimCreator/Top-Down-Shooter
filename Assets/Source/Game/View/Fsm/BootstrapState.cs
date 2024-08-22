using Model;

namespace State
{
    public class BootstrapState : FsmState
    {
        private readonly BulletViewFactory _bulletFactory;
        private readonly EnemyViewFactory _enemyFactory;
        private readonly GainViewFactory _gainFactory;
        private readonly WeaponViewFactory _weaponFactory;
        private readonly ZoneViewFactory _zoneFactory;
        private readonly ServiceLocator _serviceLocator;
        private readonly Inventary _playerInventary;

        public BootstrapState(BulletViewFactory bulletFactroy,EnemyViewFactory enemyFactory,GainViewFactory gainFactory,WeaponViewFactory weaponFactory,ZoneViewFactory zoneFactory,Inventary playerInventary,ServiceLocator locator,Fsm fsm) : base(fsm)
        {
            _bulletFactory = bulletFactroy;
            _enemyFactory = enemyFactory;
            _gainFactory = gainFactory;
            _weaponFactory = weaponFactory;
            _zoneFactory = zoneFactory;
            _serviceLocator = locator;
        }

        public override void Enter()
        {
            RegistarService();
            Fsm.SetState<LevelInachelisateState>();
        }

        private void RegistarService()
        {
            _serviceLocator.RegisterService(_bulletFactory);
            _serviceLocator.RegisterService(_enemyFactory);
            _serviceLocator.RegisterService(_gainFactory);
            _serviceLocator.RegisterService(_weaponFactory);
            _serviceLocator.RegisterService(_zoneFactory);
            _serviceLocator.RegisterService(_playerInventary);

            _serviceLocator.RegisterService(new InputRouter());
            _serviceLocator.RegisterService(new PlayerDate());
        }
    }
}
