using UnityEngine;

public class Automaton : Weapon
{
    public Automaton(Fsm fsm,Transform weapon, int damage, float bulletPerSecond, int numberBullet = 1) : base(fsm,weapon, damage, bulletPerSecond, numberBullet)
    {
    }
}