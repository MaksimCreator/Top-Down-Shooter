﻿using UnityEngine;

public class Gun : Weapon
{
    public Gun(Transform weapon, float bulletDistanse, int damage, float bulletPerSecond, int numberBullet = 1) : base(weapon, bulletDistanse, damage, bulletPerSecond, numberBullet)
    {
    }
}