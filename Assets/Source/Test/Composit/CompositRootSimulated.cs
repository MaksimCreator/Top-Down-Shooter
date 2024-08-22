using UnityEngine;
using System.Collections.Generic;

public class CompositRootSimulated : CompositRoot
{
    [SerializeField] private PlayerCompositRoot _playerCompositRoot;
    [SerializeField] private InatelisateLevelCompositRoot _inatelisateLevelCompositRoot;
    [SerializeField] private BulletViewFactory _bulletViewFactory;
    [SerializeField] private EnemyViewFactory _enemyViewFactory;
    [SerializeField] private AllSoldier _allSoldireConffig;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private EnemySpawnreConffig _enemyConffig;

    private Queue<ISimulated> _simulated = new();
    private EnemySimulation _enemySimulation;

    public BulletSimulation BulletSimulation { get; private set; }

    public override void Compos()
    {
        _simulated.Enqueue(BulletSimulation = new BulletSimulation(new SpawnerBullet(_bulletViewFactory)));

        _playerCompositRoot.Compos();
        _playerCompositRoot.enabled = true;

        _simulated.Enqueue(_enemySimulation = new EnemySimulation(_enemyViewFactory, _playerCompositRoot.Wallet, _allSoldireConffig,
            _mainCamera, _inatelisateLevelCompositRoot.MapBounds, _playerCompositRoot.Player, _enemyConffig.CooldownUpdateSpawnEnemy,
            _enemyConffig.DeltaTiemDelay, _enemyConffig.StartCooldownSpawnEnemy, _enemyConffig.MinCooldownSpawn));
    }

    public void AllStop()
    {
        OnDisable();
        _enemySimulation.AllStop();
        BulletSimulation.AllStop();
    }

    public void StopSimulated()
    {
        foreach (var item in _simulated)
            item.StopSimulate();
    }

    public void StartSimulated()
    {
        foreach (var item in _simulated)
            item.StartSimulate();
    }

    private void Update()
    {
        foreach (var simulated in _simulated)
            simulated.Update(Time.deltaTime);
    }

    private void OnEnable()
    => _enemySimulation.Enable();

    private void OnDisable()
    => _enemySimulation.Disable();
}