using UnityEngine;

public class PlayerCompositRoot : CompositRoot
{
    [SerializeField] private InatelisateLevelCompositRoot _levelCompositRoot;
    [SerializeField] private CompositRootSimulated _allSimulated;
    [SerializeField] private AllWeaponGame _allWeaponConffig;
    [SerializeField] private AllGain _allGainConffig;
    [SerializeField] private PlayerConffig _playerConffig;
    [SerializeField] private WeaponViewFactory _weaponViewFactroy;
    [SerializeField] private GainViewFactory _gainViewFactroy;
    [SerializeField] private Transform _startPositionWeapon;
    [SerializeField] private Transform _player;
    [SerializeField] private EndGameView _endGameView;
    [SerializeField] private float _cooldownSpawnWeapon;
    [SerializeField] private float _cooldownSpawnGain;
    [SerializeField] private float _cooldownDestroyBonus;

    private readonly Fsm _playerFsmMovemeng = new();
    private readonly Fsm _playerFsmAttack = new();
    private PlayerHealth _health;
    private InputRouter _router;

    public Transform WeaponStartPosition => _startPositionWeapon;
    public Wallet Wallet { get; private set; }
    public Inventary Inventary { get; private set; }
    public Player Player { get; private set; }
    public SpawnerBonus SpawnerBonus { get; private set; }

    public override void Compos()
    {
        Wallet = new Wallet();

        _health = new PlayerHealth(_playerConffig.Health);
        Player = new Player(_health,_player,_playerConffig.Speed);
        PlayerMovemeng movemeng = new PlayerMovemeng(_playerConffig.RotationPerSecond, Player,_player);

        Gun StartWeapon = new Gun(_startPositionWeapon,_allWeaponConffig.GunConffig.Damage,_allWeaponConffig.GunConffig.BulletPerSecond);
        Inventary = new Inventary(_allSimulated.BulletSimulation.Simulate,movemeng.Rotate).BindWeapon(StartWeapon);

        SpawnerBonus = new SpawnerBonus(_weaponViewFactroy, _gainViewFactroy, Inventary, _levelCompositRoot.Map,
            _startPositionWeapon,_allGainConffig,_allWeaponConffig,_cooldownSpawnWeapon,_cooldownSpawnGain,_cooldownDestroyBonus);

        SpawnerBonus.Creat(StartWeapon, _startPositionWeapon);
        InitFsm(movemeng,Inventary);

        PlayerController controller = new PlayerController(_playerFsmMovemeng,movemeng);
        InputPlayerAttack inputPlayerAttack = new InputPlayerAttack(_playerFsmAttack);

        _router = new InputRouter(controller,inputPlayerAttack);
    }

    private void OnEnable()
    { 
        _router.Enable();
        SpawnerBonus.Enable();
        _health.onDeath += () => _endGameView.Enter(Wallet.Score);
    }

    private void OnDisable()
    { 
        _router.Disable();
        SpawnerBonus.Enable();
        _health.onDeath -= () => _endGameView.Enter(Wallet.Score);
    }

    private void Update()
    => _router.Update(Time.deltaTime);

    private void InitFsm(PlayerMovemeng movemeng,Inventary playerInventary)
    {
        _playerFsmMovemeng
            .BindState(new FsmStateIdel(_playerFsmMovemeng))
            .BindState(new FsmStateMovemeng(_player,movemeng,_playerFsmMovemeng));

        _playerFsmAttack
            .BindState(new FsmStateIdel(_playerFsmAttack))
            .BindState(new FsmStateAttack(playerInventary,_playerFsmAttack));
    }
}