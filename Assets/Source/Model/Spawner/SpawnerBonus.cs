using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SpawnerBonus
{
    #region Parameters
    private readonly WeaponViewFactory _weaponViewFactory;
    private readonly GainViewFactory _gainViewFactory;
    private readonly Inventary _playerInventary;
    private readonly MapBounds _mapBounds;
    private readonly Transform _muzzelPositionWeapon;
    private readonly AllWeaponGame _allWeaponGame;
    private readonly AllGain _allGain;
    private readonly Func<Gain>[] _variantsGain;
    private readonly Func<Weapon>[] _variantsWeapon;
    private readonly WeaponVisiter _weaponVisiter;
    private readonly float _cooldownSpawnWeapon;
    private readonly float _cooldownSpawnGain;
    private readonly float _cooldownDestryBonus;

    private List<IDisposable> _disposables = new List<IDisposable>();
    #endregion

    public SpawnerBonus(WeaponViewFactory weaponViewFactory, GainViewFactory gainViewFactory, Inventary playerInventary,MapBounds map,Transform muzzelPosition,AllGain allGain,AllWeaponGame allWeaponGame, float cooldownSpawnWeapon, float cooldownSpawnGain,float cooldownDestroyBonus)
    {
        _weaponViewFactory = weaponViewFactory;
        _gainViewFactory = gainViewFactory;
        _playerInventary = playerInventary;
        _mapBounds = map;
        _muzzelPositionWeapon = muzzelPosition;
        _allWeaponGame = allWeaponGame;
        _allGain = allGain;
        _cooldownSpawnWeapon = cooldownSpawnWeapon;
        _cooldownSpawnGain = cooldownSpawnGain;
        _cooldownDestryBonus = cooldownDestroyBonus;

        _weaponVisiter = new();
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
    {
        _disposables.Add(Timer.StartInfiniteTimer(_cooldownSpawnWeapon, () =>
        {
            Func<Weapon>[] variants = GenerateNewVariants().ToArray();
            int randomVariants = Random.Range(0, variants.Length);
            Transform position = _mapBounds.GenerateRandomPositionWithinBounds();
            Weapon weapon = variants[randomVariants].Invoke();
            _weaponViewFactory.Creat(weapon, position);
            Timer.StartTimer(_cooldownDestryBonus, () => _weaponViewFactory.Destroy(weapon));
        }));

        _disposables.Add(Timer.StartInfiniteTimer(_cooldownSpawnGain, () => 
        {
            int randomVariants = Random.Range(0, _variantsGain.Length);
            Transform position = _mapBounds.GenerateRandomPositionWithinBounds();
            Gain gain = _variantsGain[randomVariants].Invoke();
            _gainViewFactory.Creat(gain, position);
            Timer.StartTimer(_cooldownDestryBonus, () => _gainViewFactory.Destroy(gain));
        }));
    }

    public void Disable()
    {
        foreach (IDisposable disposable in _disposables)
            disposable.Dispose();
    }

    public void Destroy(Weapon prefab)
    => _weaponViewFactory.Destroy(prefab);

    public void Destroy(Gain prefab)
    => _gainViewFactory.Destroy(prefab);

    #region CreatEntity
        public void Creat(Weapon weapon,Transform parent)
        => _weaponViewFactory.Creat(weapon, parent,isParent: true,IsPhysics: false);

        private void Creat(Weapon weapon)
        => _weaponViewFactory.Creat(weapon, _mapBounds.GenerateRandomPositionWithinBounds());

        private void Creat(Gain gain)
        => _gainViewFactory.Creat(gain, _mapBounds.GenerateRandomPositionWithinBounds());

        private Automaton CreatAutomationWeapon()
        => new Automaton(_muzzelPositionWeapon, _allWeaponGame.AutomatonConffig.Damage, _allWeaponGame.AutomatonConffig.BulletPerSecond,_allWeaponGame.AutomatonConffig.NumberBulletPerShoot);

        private GrenadeLauncher CreatGrenadeLauncherWeapon()
        => new GrenadeLauncher(_muzzelPositionWeapon,_allWeaponGame.GrenadeLauncherConffig.Damage,_allWeaponGame.GrenadeLauncherConffig.BulletPerSecond,_allWeaponGame.GrenadeLauncherConffig.NumberBulletPerShoot);

        private Gun CreatGunWeapon()
        => new Gun(_muzzelPositionWeapon, _allWeaponGame.GunConffig.Damage, _allWeaponGame.GunConffig.BulletPerSecond, _allWeaponGame.GunConffig.NumberBulletPerShoot);

        private Shotgun CreatShotgunWeapon()
        => new Shotgun(_muzzelPositionWeapon, _allWeaponGame.ShotgunConffig.AngelBullet, _allWeaponGame.ShotgunConffig.BulletDistance, _allWeaponGame.ShotgunConffig.Damage, _allWeaponGame.ShotgunConffig.BulletPerSecond, _allWeaponGame.ShotgunConffig.NumberBulletPerShoot);

        private Accaleration CreatAccalerationGain()
        => new Accaleration(_allGain.CooldownAccalerationGain);

        private Invulnerability CreatInvulnerabilityGain()
        => new Invulnerability(_allGain.CooldownInvulnerabilityGain);
    #endregion

    private List<Func<Weapon>> GenerateNewVariants()
    {
        List<Func<Weapon>> variants = new List<Func<Weapon>>();

        for(int i = 0; i < _variantsWeapon.Length;i++)
            variants.Add(_variantsWeapon[i]);

        _weaponVisiter.Visit((dynamic)_playerInventary.Weapon);

        for (int i = 0; i < _variantsWeapon.Length;i++)
        {
            if (_variantsWeapon[i].Method.ReturnParameter.ParameterType == _weaponVisiter.CurentWeapon)
                variants.Remove(_variantsWeapon[i]);
        }

        return variants;
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