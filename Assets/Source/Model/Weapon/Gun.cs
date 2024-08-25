using UnityEngine;

public class Gun : Weapon
{
    public Gun(Transform weapon, int damage, float bulletPerSecond, int numberBullet = 1) : base(weapon, damage, bulletPerSecond, numberBullet)
    {
    }
}