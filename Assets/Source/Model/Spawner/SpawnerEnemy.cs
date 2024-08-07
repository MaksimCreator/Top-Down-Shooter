using System;
using UnityEngine;
using Random = UnityEngine.Random; 

public class SpawnerEnemy
{
    #region Parameter
    private readonly EnemySimulation _enemySimulation;
    private readonly EnemyViewFactory _enemyViewFactory;
    private readonly Wallet _walletPlayer;
    private readonly AllSoldier _enemyConffig;
    private readonly Camera _camera;
    private readonly Transform _defolPointSpawn;
    private readonly Transform _centerOfTheCircle;
    private readonly Player _player;

    private readonly float _cooldownUpdateSpawnEnemy;
    private readonly float _deltaTimeDelay;
    private readonly float _minCooldownSpawn;

    private float _cooldownSpawnEnemy;

    private readonly EnemyVisiter _enemyVisiter;
    private readonly (Func<Enemy>,int)[] _variantsEnemy;
    private readonly Vector2 _offset = new Vector2(0.35f,0.35f);
    private readonly float _radius;

    private bool _isUpdate = false;

    private IDisposable _enemySpawner;
    private IDisposable _updateTimer;
    #endregion

    public SpawnerEnemy(EnemySimulation simulation,EnemyViewFactory factory, Wallet walletPlayer,AllSoldier allSoldire,Camera camera,Transform defolPointSpawn,Transform centerOfTheCircle,Player player, float cooldownUpdateSpawnEnemy, float startCooldownSpawnEnemy, float deltaTimeDelay, float minCooldownSpawn)
    {
        _enemySimulation = simulation;
        _enemyViewFactory = factory;
        _cooldownSpawnEnemy = startCooldownSpawnEnemy;
        _walletPlayer = walletPlayer;
        _deltaTimeDelay = deltaTimeDelay;
        _minCooldownSpawn = minCooldownSpawn;
        _enemyConffig = allSoldire;
        _camera = camera;
        _defolPointSpawn = defolPointSpawn;
        _centerOfTheCircle = centerOfTheCircle;
        _player = player;

        _radius = GetRadius();
        InitCenterOfTheCircle();

        _enemyVisiter = new EnemyVisiter(InstantiateEnemy);
        _variantsEnemy = new (Func<Enemy>, int)[]
        {
            (CreatPrivateSoldier,6),
            (CreatArmoredSoldier,9),
            (CreatNimbleSoldier,10)
        };
    }

    public void Enable()
    {
        _enemySpawner = Timer.StartInfiniteTimer(_cooldownSpawnEnemy, () =>
        {
            SpawnEnemy();
            TryUpdateTimer();
        });

        _updateTimer = Timer.StartInfiniteTimer(_cooldownUpdateSpawnEnemy, () =>
        {
            if (_cooldownSpawnEnemy - _deltaTimeDelay <= _minCooldownSpawn)
            {
                _cooldownSpawnEnemy = _minCooldownSpawn;
                _updateTimer.Dispose();
            }
            else
            {
                _cooldownSpawnEnemy -= _deltaTimeDelay;
            }

            _isUpdate = true;
        });
    }

    private Transform GenerateRandomPosition()
    {
        Vector2 positionSpawn = Random.insideUnitCircle.normalized * _radius + _offset;
        _defolPointSpawn.position = new Vector3(positionSpawn.x + _centerOfTheCircle.position.x,_centerOfTheCircle.position.y,positionSpawn.y + _centerOfTheCircle.position.z);
        return _defolPointSpawn;
    }
    
    private float GetRadius()
    {
        float length = _camera.farClipPlane;
        float width = _camera.orthographicSize * 2 * _camera.aspect;
        return (float)Math.Sqrt(length * length + width * width) / 2;
    }

    private void InitCenterOfTheCircle()
    { 
        _centerOfTheCircle.position = new Vector3(_camera.transform.position.x, _defolPointSpawn.position.y, _camera.transform.position.z);
        _centerOfTheCircle.parent = _camera.transform;
    }

    private void SpawnEnemy()
    {
        if (_camera.orthographic == false)
            throw new InvalidOperationException();

        Vector2 randomPosition = Random.insideUnitCircle.normalized * _radius + _offset;
        int randomNumber = Random.Range(1, 11);

        for (int i = 0; i < _variantsEnemy.Length; i++)
        {
            if (randomNumber <= _variantsEnemy[i].Item2)
            {
                Enemy enemy = Enable(_variantsEnemy[i].Item1.Invoke());
                _enemySimulation.Simulated(enemy);
                return;
            }
        }

        throw new InvalidOperationException();
    }

    private void TryUpdateTimer()
    {
        if (_isUpdate == false)
            return;

        _isUpdate = false;
        _enemySpawner.Dispose();
        _enemySpawner = Timer.StartInfiniteTimer(_cooldownSpawnEnemy, () =>
        {
            SpawnEnemy();
            TryUpdateTimer();
        });
    }

    private Enemy Enable(Enemy enemy)
    {
        _enemyVisiter.Visit((dynamic)enemy);
        (Enemy,GameObject) pair = _enemyVisiter.PoolObject.Enable(enemy,GenerateRandomPosition());
        pair.Item1.StartMovemeng(_player, pair.Item2.transform);
        return pair.Item1;
    }

    private void InstantiateEnemy(Enemy enemy, Transform position)
    => _enemyViewFactory.Creat(enemy, position);

    public void Disable()
    { 
        _enemySpawner.Dispose();
        _updateTimer.Dispose();
    }

    #region CreatMethod
        private ArmoredSoldier CreatArmoredSoldier()
        {
            ArmoredSoldier soldier = new ArmoredSoldier(_enemyConffig.ArmoredSoldier.Speed);
            EnemyHealth health = new EnemyHealth(_enemyConffig.ArmoredSoldier.MaxHealth, soldier);
        
            soldier.BindHealth(health);
            SubscratePerformed(health);
        
            return soldier;
        }

        private NimbleSoldier CreatNimbleSoldier()
        {
            NimbleSoldier soldier = new NimbleSoldier(_enemyConffig.NimbleSoldier.Speed);
            EnemyHealth health = new EnemyHealth(_enemyConffig.ArmoredSoldier.MaxHealth, soldier);

            soldier.BindHealth(health);
            SubscratePerformed(health);

            return soldier;
        }

        private PrivateSoldier CreatPrivateSoldier()
        {
            PrivateSoldier soldier = new PrivateSoldier(_enemyConffig.PrivateSoldier.Speed);
            EnemyHealth health = new EnemyHealth(_enemyConffig.ArmoredSoldier.MaxHealth, soldier);

            soldier.BindHealth(health);
            SubscratePerformed(health);

            return soldier;
        }

        private void SubscratePerformed(EnemyHealth health)
        => health.onDeath += _walletPlayer.onKill;
    #endregion

    private class EnemyVisiter : IEnemyVisiter
    {
        private readonly PoolObject<Enemy> _privateSoldier;
        private readonly PoolObject<Enemy> _nimbleSoldier;
        private readonly PoolObject<Enemy> _armorSoldier;

        public PoolObject<Enemy> PoolObject { get; private set; }

        public EnemyVisiter(Action<Enemy, Transform> instantiate)
        {
            _privateSoldier = new();
            _nimbleSoldier = new();
            _armorSoldier = new();

            _privateSoldier.onInstantiat += instantiate;
            _nimbleSoldier.onInstantiat += instantiate;
            _armorSoldier.onInstantiat += instantiate;
        }

        public void Visit(PrivateSoldier visit)
        => PoolObject = _privateSoldier;

        public void Visit(NimbleSoldier visit)
        => PoolObject = _nimbleSoldier;

        public void Visit(ArmoredSoldier visit)
        => PoolObject = _armorSoldier;
    }
}
