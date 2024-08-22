using UniRx;
using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SpawnerEnemy
{
    private readonly EnemySimulation _enemySimulation;
    private readonly EnemyViewFactory _enemyViewFactory;
    private readonly Wallet _walletPlayer;
    private readonly AllSoldier _enemyConffig;
    private readonly EnemyVisiter _enemyVisiter; 
    private readonly EnemyTimer _timer;
    private readonly Camera _camera;
    private readonly Implementation _implementation;
    private readonly (Func<Enemy>,int)[] _variantsEnemy;

    public SpawnerEnemy(EnemySimulation simulation,EnemyViewFactory factory, Wallet walletPlayer,AllSoldier allSoldire,Camera camera,MapBounds mapBounds,Player player, float cooldownUpdateSpawnEnemy, float startCooldownSpawnEnemy, float deltaTimeDelay, float minCooldownSpawn)
    {
        _enemySimulation = simulation;
        _enemyViewFactory = factory;
        _walletPlayer = walletPlayer;
        _enemyConffig = allSoldire;
        _camera = camera;

        _timer = new EnemyTimer(SpawnEnemy,startCooldownSpawnEnemy,cooldownUpdateSpawnEnemy, deltaTimeDelay, minCooldownSpawn);
        _enemyVisiter = new EnemyVisiter(InstantiateEnemy);
        _implementation = new Implementation(_enemyVisiter, player, _camera, mapBounds);

        _variantsEnemy = new (Func<Enemy>, int)[]
        {
            (CreatPrivateSoldier,6),
            (CreatArmoredSoldier,9),
            (CreatNimbleSoldier,10)
        };
    }

    public void Enable()
    => _timer.Enable();

    public void Disable()
    => _timer.Disable();

    public void StartTimer()
    => _timer.StartTimer();

    public void StopTimer()
    => _timer.StopTimer();

    public void Destroy(IEnumerable<Enemy> enemys)
    {
        foreach (var entity in enemys)
        {
            Enemy enemy = entity as Enemy;
            _enemyViewFactory.Destroy(enemy);
        }
    }

    private void SpawnEnemy()
    {
        if (_camera.orthographic == false)
            throw new InvalidOperationException();

        if (_enemySimulation.CanSimulated == false)
            return;

        int randomNumber = Random.Range(1, 11);

        for (int i = 0; i < _variantsEnemy.Length; i++)
        {
            if (randomNumber <= _variantsEnemy[i].Item2)
            {
                Enemy enemy = _implementation.EnableEnemy(_variantsEnemy[i].Item1.Invoke());
                _enemySimulation.Simulate(enemy);
                return;
            }
        }

        throw new InvalidOperationException();
    }
   
    private void InstantiateEnemy(Enemy enemy, Vector3 postion,Quaternion rotation)
    => _enemyViewFactory.Creat(enemy, postion,rotation,_enemyVisiter.PoolObject.AddObject);

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

    private class Implementation
    {
        private readonly EnemyVisiter _visiter;
        private readonly Player _player;
        private readonly Camera _camera;
        private readonly Vector3 _centerOfTheSpawnPosition;
        private readonly float _radius;

        private readonly Vector2 _offset = new Vector2(0.35f, 0.35f);

        public Implementation(EnemyVisiter visiter, Player player, Camera camera,MapBounds bounds)
        {
            _visiter = visiter;
            _player = player;
            _camera = camera;

            _radius = GetRadius();
            _centerOfTheSpawnPosition = bounds.GetCenterMap();
        }

        public Enemy EnableEnemy(Enemy enemy)
        {
            _visiter.TrySwitchPoolObject(enemy);
            (Enemy, GameObject) pair = _visiter.PoolObject.Enable(enemy, GenerateRandomPosition(), Quaternion.identity);

            enemy = pair.Item1;
            GameObject gameObjectEnemy = pair.Item2;
            Transform transfomEnemy = gameObjectEnemy.transform;

            SetPositionEnemy(transfomEnemy);
            enemy.StartMovemeng(_player, transfomEnemy);

            return enemy;
        }

        private Vector3 GenerateRandomPosition()
        {
            Vector2 positionSpawn = Random.insideUnitCircle.normalized * _radius + _offset;
            return new Vector3(positionSpawn.x + _centerOfTheSpawnPosition.x, _centerOfTheSpawnPosition.y, positionSpawn.y + _centerOfTheSpawnPosition.z);
        }

        private void SetPositionEnemy(Transform enemy)
        {
            Vector3 position = enemy.position;
            enemy.position = position + Vector3.up * enemy.localScale.y / 2;
        }

        private float GetRadius()
        {
            float length = _camera.farClipPlane;
            float width = _camera.orthographicSize * 2 * _camera.aspect;
            return (float)Math.Sqrt(length * length + width * width) / 2;
        }
    }

    private class EnemyVisiter : IEnemyVisiter
    {
        private readonly PoolObject<Enemy> _privateSoldier;
        private readonly PoolObject<Enemy> _nimbleSoldier;
        private readonly PoolObject<Enemy> _armorSoldier;

        public PoolObject<Enemy> PoolObject { get; private set; }

        public EnemyVisiter(Action<Enemy, Vector3, Quaternion> instantiate)
        {
            _privateSoldier = new();
            _nimbleSoldier = new();
            _armorSoldier = new();

            _privateSoldier.onInstantiat += instantiate;
            _nimbleSoldier.onInstantiat += instantiate;
            _armorSoldier.onInstantiat += instantiate;
        }

        public void TrySwitchPoolObject(Enemy enemy)
        => Visit((dynamic)enemy);

        public void Visit(PrivateSoldier visit)
        => PoolObject = _privateSoldier;

        public void Visit(NimbleSoldier visit)
        => PoolObject = _nimbleSoldier;

        public void Visit(ArmoredSoldier visit)
        => PoolObject = _armorSoldier;
    }

    private class EnemyTimer
    {
        private readonly Action _spawnEnemy;
        private readonly float _cooldownUpdateSpawnEnemy;
        private readonly float _deltaTimeDelay;
        private readonly float _minCooldownSpawn;

        private CompositeDisposable _enemySpawner;
        private CompositeDisposable _updateTime;
        private CompositeDisposable _enemyTimer;
        private CompositeDisposable _updateTimer;
        private CompositeDisposable[] _allDisposables;

        private float _cooldownSpawnEnemy;
        private float _deltaEnemySpawnTime;
        private float _deltaUpdateTime;

        private bool _isUpdate = false;

        public EnemyTimer(Action SpawnEnemy,float cooldownSpawnEnemy,float cooldownUpdateSpawnEnemy, float deltaTimeDelay, float minCooldownSpawn)
        {
            _spawnEnemy = SpawnEnemy;
            _cooldownSpawnEnemy = cooldownSpawnEnemy;
            _cooldownUpdateSpawnEnemy = cooldownUpdateSpawnEnemy;
            _deltaTimeDelay = deltaTimeDelay;
            _minCooldownSpawn = minCooldownSpawn;
        }

        public void Enable()
        {
            _enemySpawner = Timers.StartInfiniteTimer(_cooldownSpawnEnemy, () =>
            {
                _spawnEnemy();
                TryUpdateTimer();
            });
            _updateTime = Timers.StartInfiniteTimer(_cooldownUpdateSpawnEnemy, () =>
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

            _enemyTimer = Timers.StartTimer();
            _updateTimer = Timers.StartTimer();
        }

        public void Disable()
        {
            _enemySpawner.Clear();
            _enemySpawner.Clear();
            _updateTimer.Clear();
            _updateTime.Clear();
        }

        public void StopTimer()
        {
            _allDisposables = new CompositeDisposable[2]
            {
                _enemyTimer,
                _updateTimer,
            };
            
            _deltaEnemySpawnTime = Timers.GetTime(_enemyTimer) % _cooldownSpawnEnemy;
            _deltaUpdateTime = Timers.GetTime(_updateTimer) % _cooldownUpdateSpawnEnemy;

            Timers.StopRange(_allDisposables);

            Disable();
        }

        public void StartTimer()
        {
            Timers.StartTimer(_deltaEnemySpawnTime,() =>
            {
                _enemySpawner = Timers.StartInfiniteTimer(_cooldownSpawnEnemy, () =>
                {
                    _spawnEnemy();
                    TryUpdateTimer();
                });
            });

            Timers.StartTimer(_deltaUpdateTime, () =>
            {
                _updateTime = Timers.StartInfiniteTimer(_cooldownUpdateSpawnEnemy, () =>
                {
                    if (_cooldownSpawnEnemy - _deltaTimeDelay <= _minCooldownSpawn)
                    {
                        _cooldownSpawnEnemy = _minCooldownSpawn;
                        _updateTimer.Clear();
                    }
                    else
                    {
                        _cooldownSpawnEnemy -= _deltaTimeDelay;
                    }

                    _isUpdate = true;
                });
            });

            _enemyTimer = Timers.StartTimer(_deltaEnemySpawnTime);
            _updateTimer = Timers.StartTimer(_deltaUpdateTime);
        }

        private void TryUpdateTimer()
        {
            if (_isUpdate == false)
                return;

            _isUpdate = false;
            _enemySpawner.Clear();
            _enemySpawner = Timers.StartInfiniteTimer(_cooldownSpawnEnemy, () =>
            {
                _spawnEnemy();
                TryUpdateTimer();
            });
        }
    }
}