using UnityEngine;

[CreateAssetMenu(menuName = "Conffig/Defolt Weapon")]
public class WeaponConffig : ScriptableObject
{
    [SerializeField] private float _bulletPerSecond; 
    [SerializeField] private int _damage; 
    [SerializeField] private int _numberBulletPerShoot = 1;

    public float BulletPerSecond => _bulletPerSecond;
    public int Damage => _damage;
    public int NumberBulletPerShoot => _numberBulletPerShoot;
}