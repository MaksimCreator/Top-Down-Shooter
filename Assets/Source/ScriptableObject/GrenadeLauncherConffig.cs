using UnityEngine;

[CreateAssetMenu(menuName = "Conffig/Grenade Weapon")]
public class GrenadeLauncherConffig : WeaponConffig
{
    [SerializeField] private int _explosionRadius;

    public int ExplosionRadius => _explosionRadius;
}