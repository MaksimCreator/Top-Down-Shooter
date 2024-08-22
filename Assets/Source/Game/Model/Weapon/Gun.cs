using UnityEngine;

public class Gun : Weapon
{
    public Gun(Fsm fsm,Transform weapon, int damage, float bulletPerSecond, int numberBullet = 1) : base(fsm,weapon, damage, bulletPerSecond, numberBullet)
    {
    }
}