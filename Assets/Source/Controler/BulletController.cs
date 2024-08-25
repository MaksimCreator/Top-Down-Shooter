using Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using View;

public class BulletController : IDeltaUpdatable
{
    private readonly Dictionary<IBulletData, IUpdatable> _bullets = new();
    private readonly BulletSimulation _bulletSimulation;
    private readonly Inventary _inventary;

    public float BulletPerSecond => _inventary.Weapon.BulletPerSecond;
    public ISimulated Simulated => _bulletSimulation;

    public BulletController(BulletSimulation bulletSimulation,Inventary inventary)
    {
        _bulletSimulation = bulletSimulation;
        _inventary = inventary;
    }

    public void Enable()
    => _inventary.Weapon.onShoot += _bulletSimulation.Simulate;

    public void Disable()
    => _inventary.Weapon.onShoot -= _bulletSimulation.Simulate;

    public void Shoot(Vector3 direction)
    { 
        _inventary.Weapon.Shoot(direction);

        if (_bulletSimulation.CurentBullet == null)
            throw new InvalidOperationException();

        IBulletData bulletData = _bulletSimulation.CurentBullet;

        if (_bullets.TryGetValue(bulletData, out IUpdatable IUpdatebleFsm) == false)
        {
            Fsm fsm = new Fsm();

            fsm.BindState(new BulletIdelState(bulletData,fsm))
                .BindState(new BulletMovemengState(bulletData.BulletTransform, bulletData, bulletData, fsm))
                .BindState(new BulletDestroyState(() => _bulletSimulation.Stop(bulletData),fsm));

            fsm.SetState<BulletIdelState>();

            _bullets.Add(bulletData,fsm);
        }
    }

    public void Update(float delta)
    {
        foreach (var fsm in _bullets.Values)
            fsm.Update();

        _bulletSimulation.Update(delta);
    }
}