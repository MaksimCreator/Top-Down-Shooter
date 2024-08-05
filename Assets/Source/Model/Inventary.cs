using System;
using UnityEngine;

public class Inventary
{
    public Weapon Weapon { get; private set; }

    private Action<Bullet, Transform> _creatBullet;

    public Inventary BindWeapon(Weapon weapon,Action<Bullet, Transform> creatBullet)
    {
        if(_creatBullet != null)
            Weapon.onShoot -= creatBullet;

        _creatBullet = creatBullet;
        Weapon = weapon;
        Weapon.onShoot += creatBullet;

        return this;
    }
}
