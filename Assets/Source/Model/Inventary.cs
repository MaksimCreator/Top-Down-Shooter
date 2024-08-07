using System;
using UnityEngine;

public class Inventary
{
    public Weapon Weapon { get; private set; }

    private readonly Action<Bullet, Transform,Vector3> _creatBullet;
    private readonly Action<Vector3> _rotation;

    public Inventary(Action<Bullet, Transform, Vector3> creatBullet, Action<Vector3> rotation)
    {
        _creatBullet = creatBullet;
        _rotation = rotation;
    }

    public Inventary BindWeapon(Weapon weapon)
    {
        if(Weapon != null)
            Weapon.onShoot -= Shoot;

        Weapon = weapon;
        Weapon.onShoot += Shoot;

        return this;
    }

    private void Shoot(Bullet bullet,Transform transfom)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) == false)
            return;

        Vector3 targetPosition = hit.point;

        _creatBullet.Invoke(bullet, transfom, targetPosition);
        _rotation.Invoke(targetPosition);
    }
}