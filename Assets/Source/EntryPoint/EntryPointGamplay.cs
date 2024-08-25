using GameControllers;
using Managers;
using Model;
using State;
using UnityEngine;

// EntryPoint Activated Manager and Inachelisate Component, Manager Inachelisate Conponent and Update Controller, Controller Update Model and View
// EntryPoint -> Manager -> Controller -> View && Model

public class EntryPointGamplay : MonoBehaviour
{
    [SerializeField] private Transform _playerTransfom;
    [SerializeField] private Transform _weaponSpawn;
    [SerializeField] private Transform _map;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Camera _camera;

    private readonly ServiceLocator _serviceLocator = new();
    private readonly Wallet _wallet = new();
    private readonly PlayerHealth _health = new();
    private readonly Inventary _inventary = new();
    private readonly Fsm _fsmPlayerMovemeng = new();
    private readonly Fsm _fsmPlayerAttack = new();

    public ServiceLocator ServiceLocator => _serviceLocator;

    private void Awake()
    {
        _gameManager.ServiceLocatorInachelisate(_camera,_serviceLocator, _map);

        PlayerInit(out Player player,out PlayerMovemeng playerMovemeng, out InputRouter router, out BulletController bulletController,out BulletSimulation bulletSimulation, out PlayerController playerController, out SpawnerBonus spawnerBonus);
        EnemyInit(player,out EnemyController controller);

        InitFsmPlayer(router,bulletController,playerMovemeng);

        _enemyManager.Activate(controller);
        _playerManager.Activate(playerController, bulletController, _serviceLocator);
        _gameManager.Activate(_inventary,_wallet, _health,spawnerBonus,bulletSimulation,_weaponSpawn, _serviceLocator);
    }

    private void InitFsmPlayer(InputRouter router,BulletController bulletController,PlayerMovemeng movemeng)
    {
        _fsmPlayerMovemeng.BindState(new PlayerIdelState(router,_fsmPlayerMovemeng))
            .BindState(new PlayerMovemengState(_playerTransfom,movemeng,router,movemeng,_fsmPlayerMovemeng));

        _fsmPlayerAttack.BindState(new PlayerAttackIdelState(router, _fsmPlayerAttack))
            .BindState(new PlayerAttackState(bulletController,_serviceLocator,_fsmPlayerAttack));

        _fsmPlayerMovemeng.SetState<PlayerIdelState>();
        _fsmPlayerAttack.SetState<PlayerAttackIdelState>();
    }

    private void PlayerInit(out Player player,out PlayerMovemeng playerMovemeng,out InputRouter router,
        out BulletController bulletController,out BulletSimulation bulletSimulation,out PlayerController playerController,
        out SpawnerBonus spawnerBonus)
    {
        spawnerBonus = new SpawnerBonus(_serviceLocator,_inventary, _weaponSpawn);
        bulletSimulation = new BulletSimulation(new SpawnerBullet(_serviceLocator));
        router = _serviceLocator.GetSevice<InputRouter>();

        PlayerConffig playerConffig = _serviceLocator.GetSevice<PlayerConffig>();
        player = new Player(_health, _playerTransfom, playerConffig.Speed);
        playerMovemeng = new PlayerMovemeng(playerConffig.RotationPerSecond, player, _playerTransfom, _fsmPlayerMovemeng);

        playerController = new PlayerController(_serviceLocator, playerMovemeng, _camera, _fsmPlayerMovemeng, _fsmPlayerAttack);
        bulletController = new BulletController(bulletSimulation, _inventary);

        AllWeaponGameConffig allWeaponGameConffig = _serviceLocator.GetSevice<AllWeaponGameConffig>();

        Gun gun = new Gun(_weaponSpawn, allWeaponGameConffig.GunConffig.Damage, allWeaponGameConffig.GunConffig.BulletPerSecond);
        _serviceLocator.GetSevice<WeaponViewFactory>().Creat(gun,_weaponSpawn.position,_weaponSpawn.rotation);

        _inventary.BindWeapon(gun);
    }
   
    private void EnemyInit(Player player,out EnemyController controller)
    {
        controller = new EnemyController(_serviceLocator, _wallet, _camera, player);
    }
}