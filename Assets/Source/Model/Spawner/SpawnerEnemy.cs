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
    private readonly AllSoldierConffig _enemyConffig;

    private readonly EnemyVisiter _enemyVisiter; 
    private readonly EnemyTimer _timer;
    private readonly Camera _camera;
    private readonly Implementation _implementation;
    private readonly (Func<Enemy>,int)[] _variantsEnemyAndChanceToSpawn;

    private readonly Func<bool> _canSimulated;
    private readonly Action<Enemy> _simulatedEnemy;

    public SpawnerEnemy(Action<Enemy> simulatedEnemy,Func<bool> canSimulated,EnemySimulation simulation,EnemyViewFactory factory, Wallet walletPlayer,AllSoldierConffig allSoldire,Camera camera,IMapBoundsService mapBounds,Player player,EnemySpawnreConffig enemySpawnConffig)
    {
        _simulatedEnemy = simulatedEnemy;
        _enemySimulation = simulation;
        _enemyViewFactory = factory;
        _walletPlayer = walletPlayer;
        _enemyConffig = allSoldire;
        _camera = camera;
        _canSimulated = canSimulated;

        _timer = new EnemyTimer(SpawnEnemy,enemySpawnConffig.StartCooldownSpawnEnemy,enemySpawnConffig.CooldownUpdateSpawnEnemy,
            enemySpawnConffig.DeltaTimeDelay, enemySpawnConffig.MinCooldownSpawn);
        _enemyVisiter = new EnemyVisiter(InstantiateEnemy);
        _implementation = new Implementation(_enemyVisiter, player, _camera, mapBounds);

        _variantsEnemyAndChanceToSpawn = new (Func<Enemy>, int)[]
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
        foreach (var enemy in enemys)
        {
            _enemyViewFactory.Destroy(enemy);
            enemy.Health.onDeath -= (enemy) => SubscratePerformed(enemy, enemy.Health);
        }
    }

    private void SpawnEnemy()
    {
        if (_camera.orthographic == false)
            throw new InvalidOperationException();

        if (_canSimulated.Invoke())
            return;

        int randomNumber = Random.Range(1, 11);

        for (int i = 0; i < _variantsEnemyAndChanceToSpawn.Length; i++)
        {
            if (randomNumber <= _variantsEnemyAndChanceToSpawn[i].Item2)
            {
                Enemy enemy = _implementation.EnableEnemy(_variantsEnemyAndChanceToSpawn[i].Item1.Invoke());
                _simulatedEnemy.Invoke(enemy);
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
        health.onDeath += (enemy) => SubscratePerformed(enemy, enemy.Health);

        return soldier;
    }

    private NimbleSoldier CreatNimbleSoldier()
    {
        NimbleSoldier soldier = new NimbleSoldier(_enemyConffig.NimbleSoldier.Speed);
        EnemyHealth health = new EnemyHealth(_enemyConffig.ArmoredSoldier.MaxHealth, soldier);

        soldier.BindHealth(health);
        health.onDeath += (enemy) => SubscratePerformed(enemy,enemy.Health);

        return soldier;
    }

    private PrivateSoldier CreatPrivateSoldier()
    {
        PrivateSoldier soldier = new PrivateSoldier(_enemyConffig.PrivateSoldier.Speed);
        EnemyHealth health = new EnemyHealth(_enemyConffig.ArmoredSoldier.MaxHealth, soldier);

        soldier.BindHealth(health);
        health.onDeath += (enemy) => SubscratePerformed(enemy, enemy.Health);

        return soldier;
    }

    private void SubscratePerformed(Enemy enemy, EnemyHealth health)
    {
        _enemyVisiter.TrySwitchPoolObject(enemy);
        health.onDeath += _enemyVisiter.PoolObject.Disable;
        health.onDeath += _walletPlayer.onKill;
    }

    #endregion

    private class Implementation
    {
        private readonly EnemyVisiter _visiter;
        private readonly Player _player;
        private readonly Camera _camera;
        private readonly Vector3 _centerOfTheSpawnPosition;
        private readonly float _radius;

        private readonly Vector2 _offset = new Vector2(0.35f, 0.35f);

        public Implementation(EnemyVisiter visiter, Player player, Camera camera,IMapBoundsService bounds)
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

    private class EnemyTimer : EntitySpawnTimer
    {
        private readonly Action _spawnEnemy;
        private readonly float _cooldownUpdateSpawnEnemy;
        private readonly float _deltaTimeDelay;
        private readonly float _minCooldownSpawn;

        private CompositeDisposable _enemySpawner;
        private CompositeDisposable _updateTime;
        private CompositeDisposable _enemyTimer;
        private CompositeDisposable _updateTimer;
        private (CompositeDisposable, float, float)[] _compositDisposableByTimeByDivisor;

        private float _cooldownSpawnEnemy;
        private float _timeBefirceCreatEnemy;
        private float _timeBeforcUpdateCreatEnemy;

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
            _compositDisposableByTimeByDivisor = new (CompositeDisposable, float, float)[2]
            {
                (_enemyTimer,_timeBefirceCreatEnemy,_cooldownSpawnEnemy),
                (_updateTimer,_timeBeforcUpdateCreatEnemy,_cooldownUpdateSpawnEnemy)
            };

            StopTimer(ref _compositDisposableByTimeByDivisor);

            Disable();
        }

        public void StartTimer()
        {
            Timers.StartTimer(_timeBefirceCreatEnemy,() =>
            {
                _spawnEnemy();
                TryUpdateTimer();
                _enemySpawner = Timers.StartInfiniteTimer(_cooldownSpawnEnemy, () =>
                {
                    _spawnEnemy();
                    TryUpdateTimer();
                });
            });

            Timers.StartTimer(_timeBeforcUpdateCreatEnemy, () =>
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

            _enemyTimer = Timers.StartTimer(_timeBefirceCreatEnemy);
            _updateTimer = Timers.StartTimer(_timeBeforcUpdateCreatEnemy);
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