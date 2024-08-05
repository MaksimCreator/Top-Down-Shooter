using UnityEngine;

public class Automaton : Weapon
{
    public Automaton(Transform weapon, int damage, float bulletPerSecond, int numberBullet = 1) : base(weapon, damage, bulletPerSecond, numberBullet)
    {
    }
}