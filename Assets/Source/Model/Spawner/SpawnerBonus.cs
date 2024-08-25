using UniRx;
using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Model;

public class SpawnerBonus
{
    #region Parameters
    private readonly WeaponViewFactory _weaponViewFactory;
    private readonly GainViewFactory _gainViewFactory;
    private readonly Inventary _playerInventary;
    private readonly AllWeaponGameConffig _allWeaponGame;
    private readonly AllGainConffig _allGain;
    private readonly Transform _muzzelPositionWeapon;
    private readonly IMapBoundsService _mapBounds;

    private readonly Func<Gain>[] _variantsGain;
    private readonly Func<Weapon>[] _variantsWeapon;
    private readonly WeaponVisiter _weaponVisiter;
    private readonly EnemyTimer _enemyTimer;

    #endregion

    public SpawnerBonus(ServiceLocator locator,Inventary inventary,Transform muzzelPosition)
    {
        _playerInventary = inventary;
        _allGain = locator.GetSevice<AllGainConffig>();
        _allWeaponGame = locator.GetSevice<AllWeaponGameConffig>();
        _weaponViewFactory = locator.GetSevice<WeaponViewFactory>();
        _gainViewFactory = locator.GetSevice<GainViewFactory>();
        _mapBounds = locator.GetSevice<IMapBoundsService>();

        _muzzelPositionWeapon = muzzelPosition;

        _weaponVisiter = new WeaponVisiter();
        _enemyTimer = new EnemyTimer(CreatWeapon,CreatGain,DestroyWeapon,DestroyGain, locator.GetSevice<BonusSpawnerConffig>());
        _variantsGain = new Func<Gain>[]
        {
            CreatAccalerationGain,
            CreatInvulnerabilityGain
        };
        _variantsWeapon = new Func<Weapon>[]
        {
            CreatAutomationWeapon,
            CreatGrenadeLauncherWeapon,
            CreatGunWeapon,
            CreatShotgunWeapon
        };
    }

    public void Enable()
    => _enemyTimer.Enable();

    public void Disable()
    => _enemyTimer.Disable();

    public void StopTimer()
    => _enemyTimer.StopTimer();

    public void StartTimer()
    => _enemyTimer.StartTimer();

    #region CreatEntity


    private Weapon CreatWeapon()
    {
        Func<Weapon>[] variants = GenerateNewVariants();
        int randomVariants = Random.Range(0, variants.Length);

        Vector3 position = _mapBounds.GenerateRandomPositionWithinBounds();
        Weapon weapon = variants[randomVariants].Invoke();

        _weaponViewFactory.Creat(weapon, position, Quaternion.identity);

        return weapon;
    }

    private Gain CreatGain()
    {
        int randomVariants = Random.Range(0, _variantsGain.Length);
        Vector3 position = _mapBounds.GenerateRandomPositionWithinBounds();

        Gain gain = _variantsGain[randomVariants].Invoke();
        _gainViewFactory.Creat(gain, position, Quaternion.identity);

        return gain;
    }

    private void DestroyWeapon(Weapon weapon)
    => _weaponViewFactory.Destroy(weapon);

    private void DestroyGain(Gain gain)
    => _gainViewFactory.Destroy(gain);

    private Automaton CreatAutomationWeapon()
    => new Automaton(_muzzelPositionWeapon, _allWeaponGame.AutomatonConffig.Damage, _allWeaponGame.AutomatonConffig.BulletPerSecond,_allWeaponGame.AutomatonConffig.NumberBulletPerShoot);

    private GrenadeLauncher CreatGrenadeLauncherWeapon()
    => new GrenadeLauncher(_allWeaponGame.GrenadeLauncherConffig.ExplosionRadius,_muzzelPositionWeapon,_allWeaponGame.GrenadeLauncherConffig.Damage,_allWeaponGame.GrenadeLauncherConffig.BulletPerSecond,_allWeaponGame.GrenadeLauncherConffig.NumberBulletPerShoot);

    private Gun CreatGunWeapon()
    => new Gun(_muzzelPositionWeapon, _allWeaponGame.GunConffig.Damage, _allWeaponGame.GunConffig.BulletPerSecond, _allWeaponGame.GunConffig.NumberBulletPerShoot);

    private Shotgun CreatShotgunWeapon()
    => new Shotgun(_muzzelPositionWeapon, _allWeaponGame.ShotgunConffig.AngelBullet, _allWeaponGame.ShotgunConffig.BulletDistance, _allWeaponGame.ShotgunConffig.Damage, _allWeaponGame.ShotgunConffig.BulletPerSecond, _allWeaponGame.ShotgunConffig.NumberBulletPerShoot);

    private Accaleration CreatAccalerationGain()
    => new Accaleration(_allGain.CooldownAccalerationGain);

    private Invulnerability CreatInvulnerabilityGain()
    => new Invulnerability(_allGain.CooldownInvulnerabilityGain);

    
    #endregion

    private Func<Weapon>[] GenerateNewVariants()
    {
        int curentVariants = 0;
        Func<Weapon>[] variants = new Func<Weapon>[_variantsWeapon.Length - 1];

        _weaponVisiter.Visit((dynamic)_playerInventary.Weapon);

        for (int i = 0; i < _variantsWeapon.Length;i++)
        {
            if (_variantsWeapon[i].Method.ReturnParameter.ParameterType == _weaponVisiter.CurentWeapon == false)
            {
                variants[curentVariants] = _variantsWeapon[i];
                curentVariants++;
            }
        }

        return variants;
    }

    private class EnemyTimer : EntitySpawnTimer
    {
        private readonly Func<Weapon> _creatWeapon;
        private readonly Func<Gain> _creatGain;
        private readonly Action<Weapon> _destroyWeapon;
        private readonly Action<Gain> _destroyGain;

        private readonly BonusSpawnerConffig _bonusSpawnerConffig;

        private List<CompositeDisposable> _allCompositDisposable = new List<CompositeDisposable>();

        private CompositeDisposable _destroyWeaponTimer;
        private CompositeDisposable _destroyGainTimer;
        private CompositeDisposable _weaponTimer;
        private CompositeDisposable _gainTimer;
        private (CompositeDisposable, float, float)[] _compositDisposableByTimeByDivisor;

        private float _timeBeforeCreatWeapon;
        private float _timeBeforeCreatGain;

        public EnemyTimer(Func<Weapon> creatWeapon, Func<Gain> creatGain, Action<Weapon> destroyWeapon, Action<Gain> destroyGain, BonusSpawnerConffig bonusSpawnerConffig)
        {
            _bonusSpawnerConffig = bonusSpawnerConffig;
            _destroyWeapon = destroyWeapon;
            _creatWeapon = creatWeapon;
            _destroyGain = destroyGain;
            _creatGain = creatGain;
        }

        public void Enable()
        {
            StartCreatMethodWeapon();
            StartCreatMethodGain();
            StartTimers();
            InitAllCompositDisposable();
        }

        public void Disable()
        {
            for (int i = 0; i < _allCompositDisposable.Count; i++)
            {
                _allCompositDisposable[i].Clear();
                _allCompositDisposable[i] = new();
            }

            _allCompositDisposable = new();
        }

        public void StopTimer()
        {
            _compositDisposableByTimeByDivisor = new (CompositeDisposable, float, float)[2]
            {
                (_destroyWeaponTimer,_timeBeforeCreatWeapon,_bonusSpawnerConffig.CooldownSpawnWeapon),
                (_destroyGainTimer,_timeBeforeCreatGain,_bonusSpawnerConffig.CooldownSpawnGain)
            };

            StopTimer(ref _compositDisposableByTimeByDivisor);

            Disable();
        }

        public void StartTimer()
        {
            _allCompositDisposable.Add(Timers.StartTimer(_timeBeforeCreatWeapon, StartCreatMethodWeapon));
            StartTimers();
            InitAllCompositDisposable();
        }

        private void StartTimers()
        {
            _allCompositDisposable.Add(_weaponTimer = Timers.StartTimer());
            _allCompositDisposable.Add(_gainTimer = Timers.StartTimer());
        }

        private void InitAllCompositDisposable()
        {
            _allCompositDisposable.Add(_destroyGainTimer);
            _allCompositDisposable.Add(_destroyWeaponTimer);
        }

        private void StartCreatMethodWeapon()
        {
            _allCompositDisposable.Add(Timers.StartInfiniteTimer(_bonusSpawnerConffig.CooldownSpawnWeapon, () =>
            {
                Weapon weapon = _creatWeapon.Invoke();
                _destroyWeaponTimer = Timers.StartTimer(_bonusSpawnerConffig.CooldownDestryBonus, () => _destroyWeapon.Invoke(weapon));
            }));
        }

        private void StartCreatMethodGain()
        {
            _allCompositDisposable.Add(Timers.StartInfiniteTimer(_bonusSpawnerConffig.CooldownSpawnGain, () =>
            {
                Gain gain = _creatGain.Invoke();
                _destroyGainTimer = Timers.StartTimer(_bonusSpawnerConffig.CooldownDestryBonus, () => _destroyGain.Invoke(gain));
            }));
        }
    }

    private class WeaponVisiter : IWeaponVisiter
    {
        public Type CurentWeapon { get; private set; }

        public void Visit(Automaton visit)
        => CurentWeapon = typeof(Automaton);

        public void Visit(Gun visit)
        => CurentWeapon = typeof(Gun);

        public void Visit(Shotgun visit)
        => CurentWeapon = typeof(Shotgun);

        public void Visit(GrenadeLauncher visit)
        => CurentWeapon = typeof(GrenadeLauncher);
    }
}
