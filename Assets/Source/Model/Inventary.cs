using System;
using UnityEngine;

public class Inventary
{
    private Weapon _weapon;
    private Action<Bullet, Vector3, Vector3> _creatBullet;

    public Inventary BindWeapon(Weapon weapon,Action<Bullet, Vector3, Vector3> creatBullet)
    {
        if(_creatBullet != null)
            _weapon.onShoot -= creatBullet;

        _weapon = weapon;
        _creatBullet = creatBullet;
        _weapon.onShoot += creatBullet;

        return this;
    }
}
