using UnityEngine;

public class PlayerCompositRoot : CompositRoot
{
    [SerializeField] private CompositRootSimulated _allSimulated;
    [SerializeField] private AllWeaponGame _allWeapon;
    [SerializeField] private WeaponViewFactory _weaponViewFactroy;
    [SerializeField] private PlayerConffig _playerConffig;
    [SerializeField] private Transform _positionWeapon;
    [SerializeField] private Transform _player;
    [SerializeField] private EndGameView _endGameView;

    private Fsm _playerFsmMovemeng = new();
    private Fsm _playerFsmRotate = new();
    private Fsm _playerFsmAttack = new();
    private PlayerHealth _health;
    private InputRouter _router;

    public Wallet Wallet { get; private set; }
    public Inventary Inventary { get; private set; }

    public override void Compos()
    {
        Wallet = new Wallet();

        Gun StartWeapon = new Gun(_positionWeapon,_allWeapon.GunConffig.Damage,_allWeapon.GunConffig.BulletPerSecond);
        _weaponViewFactroy.Creat(StartWeapon, _positionWeapon,isParent:true);

        Inventary = new Inventary().BindWeapon(StartWeapon,_allSimulated.BulletSimulation.Simulate);
        _health = new PlayerHealth(_playerConffig.Health);
        Player player = new Player(_health,_playerConffig.Speed);
        PlayerMovemeng movemeng = new PlayerMovemeng(_playerConffig.RotationPerSecond, _playerFsmRotate, player);

        InitFsm(movemeng,Inventary);

        PlayerController controller = new PlayerController(_playerFsmMovemeng,movemeng);
        InputPlayerAttack inputPlayerAttack = new InputPlayerAttack(_playerFsmAttack);
        _router = new InputRouter(controller,inputPlayerAttack);
    }

    private void OnEnable()
    { 
        _router.Enable();
        _health.onDeath += () => _endGameView.Enter(Wallet.Score);
    }

    private void OnDisable()
    { 
        _router.Disable();
        _health.onDeath -= () => _endGameView.Enter(Wallet.Score);
    }

    private void Update()
    => _router.Update(Time.deltaTime);

    private void InitFsm(PlayerMovemeng movemeng,Inventary playerInventary)
    {
        _playerFsmMovemeng
            .BindState(new FsmStateIdel(_playerFsmMovemeng))
            .BindState(new FsmStateMovemeng(_player,movemeng,_playerFsmMovemeng));

        _playerFsmRotate
            .BindState(new FsmStateIdel(_playerFsmRotate))
            .BindState(new FsmStateRotate(movemeng,_playerFsmRotate));

        _playerFsmAttack
            .BindState(new FsmStateIdel(_playerFsmAttack))
            .BindState(new FsmStateAttack(playerInventary,_playerFsmAttack));

    }
}