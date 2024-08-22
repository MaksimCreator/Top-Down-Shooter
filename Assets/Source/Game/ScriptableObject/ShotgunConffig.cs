using UnityEngine;

[CreateAssetMenu(menuName = "Conffig/Shotgun Weapon")]
public class ShotgunConffig : WeaponConffig
{
    [SerializeField] private float _angelBullet;
    [SerializeField] private int _bulletDistance;

    public float AngelBullet => _angelBullet;
    public int BulletDistance => _bulletDistance;
}