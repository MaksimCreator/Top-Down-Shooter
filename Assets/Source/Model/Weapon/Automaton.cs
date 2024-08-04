using UnityEngine;

public class Automaton : Weapon
{
    public Automaton(Transform weapon, float bulletDistanse, int damage, float bulletPerSecond, int numberBullet = 1) : base(weapon, bulletDistanse, damage, bulletPerSecond, numberBullet)
    {
    }
}