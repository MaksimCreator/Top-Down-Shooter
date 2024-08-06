using System;

public class SpawnerEnemy
{
    private readonly EnemySimulation _enemySimulation;
    private readonly EnemyViewFactory _enemyViewFactory;
    private readonly Wallet _walletPlayer;
    private readonly AllSoldier _enemyConffig;
    private readonly EnemyVisiter _enemyVisiter;
    private readonly float _cooldownUpdateSpawnEnemy;
    private readonly float _deltaTimeDelay;
    private readonly float _minCooldownSpawn;

    private float _cooldownSpawnEnemy;

    private IDisposable _enemySpawner;
    private IDisposable _enemyUpdate;

    public SpawnerEnemy(EnemySimulation simulation,EnemyViewFactory factory, Wallet walletPlayer,float cooldownUpdateSpawnEnemy, float cooldownSpawnEnemy, float deltaTimeDelay, float minCooldownSpawn)
    {
        _enemySimulation = simulation;
        _enemyViewFactory = factory;
        _cooldownSpawnEnemy = cooldownSpawnEnemy;
        _walletPlayer = walletPlayer;
        _deltaTimeDelay = deltaTimeDelay;
        _minCooldownSpawn = minCooldownSpawn;
        _enemyVisiter = new EnemyVisiter();
    }

    public void Enable()
    {
        _enemySpawner = Timer.StartInfiniteTimer(_cooldownSpawnEnemy, () => Spawn());

        _enemyUpdate = Timer.StartInfiniteTimer(_cooldownUpdateSpawnEnemy, () =>
        {
            if (_cooldownSpawnEnemy -= _deltaTimeDelay <= _minCooldownSpawn)
            {

            }
        });
    }

    private void Spawn()
    {

    }

    public Enemy Enable(Enemy enemy)
    {
        _enemyVisiter.Visit((dynamic)enemy);
        _enemyVisiter.PoolObject.Enable()
    }

    public void Disable()
    { 
        _enemySpawner.Dispose();
        _enemyUpdate.Dispose();
    }

    private ArmoredSoldier CreatArmoredSoldier()
    {
        ArmoredSoldier soldier = new ArmoredSoldier(_enemyConffig.ArmoredSoldier.Speed);
        soldier.BindHealth(new EnemyHealth(_enemyConffig.ArmoredSoldier.MaxHealth, soldier));
        return soldier;
    }

    private NimbleSoldier CreatNimbleSoldier()
    {
        NimbleSoldier soldier = new NimbleSoldier(_enemyConffig.NimbleSoldier.Speed);
        soldier.BindHealth(new EnemyHealth(_enemyConffig.NimbleSoldier.MaxHealth, soldier));
        return soldier;
    }

    private PrivateSoldier CreatPrivateSoldier()
    {
        PrivateSoldier soldier = new PrivateSoldier(_enemyConffig.PrivateSoldier.Speed);
        soldier.BindHealth(new EnemyHealth(_enemyConffig.PrivateSoldier.MaxHealth, soldier));
        return soldier;
    }

    private class EnemyVisiter : IEnemyVisiter
    {
        private readonly PoolObject<Enemy> _privateSoldier;
        private readonly PoolObject<Enemy> _nimbleSoldier;
        private readonly PoolObject<Enemy> _armorSoldier;

        public PoolObject<Enemy> PoolObject { get; private set; }

        public void Visit(PrivateSoldier visit)
        => PoolObject = _privateSoldier;

        public void Visit(NimbleSoldier visit)
        => PoolObject = _nimbleSoldier;

        public void Visit(ArmoredSoldier visit)
        => PoolObject = _armorSoldier;
    }
}
