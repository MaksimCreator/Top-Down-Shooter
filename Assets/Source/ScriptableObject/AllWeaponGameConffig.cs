using UnityEngine;

[CreateAssetMenu(menuName = "Conffig/AllWeapon")]
public class AllWeaponGameConffig : ScriptableObject,IService
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
