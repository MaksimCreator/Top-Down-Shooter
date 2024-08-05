using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Conffig/Defolt Weapon")]
public class WeaponConffig : ScriptableObject
{
    [SerializeField] private float _bulletDistanse;
    [SerializeField] private float _bulletPerSecond; 
    [SerializeField] private int _damage; 
    [SerializeField] private int _numberBulletPerShoot = 1;

    public float BulletDistance => _bulletDistanse;
    public float BulletPerSecond => _bulletPerSecond;
    public int Damage => _damage;
    public int NumberBulletPerShoot => _numberBulletPerShoot;
}
[CreateAssetMenu(menuName = "Conffig/AllWeapon")]
public class AllWeaponGame : ScriptableObject
{
    [SerializeField] private WeaponConffig _automatonConffig;
    [SerializeField] private WeaponConffig _gunConffig;
    [SerializeField] private GrenadeLauncherConffig _grenadeLauncherConffig;
    [SerializeField] private ShotgunConffig _shotgunConffig;

    public WeaponConffig AutomatonConffig => _automatonConffig;
    public WeaponConffig GunConffig => _gunConffig;
    public GrenadeLauncherConffig GrenadeLauncherConffig => _grenadeLauncherConffig;
    public ShotgunConffig ShotgunConffig => _shotgunConffig; 
}