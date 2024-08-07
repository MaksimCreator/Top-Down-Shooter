using System.Collections.Generic;
using System;
using UnityEngine;

public class BulletSimulation : Simulated<Bullet>
{
    private readonly SpawnerBullet _spawner;

    public BulletSimulation(SpawnerBullet spawner)
    {
        _spawner = spawner;

        OnDistroy += Distroy;
    }

    public void Simulate(Bullet bullet, Transform startPosition,Vector3 targetPosition)
    {
        Bullet curentBullet = _spawner.Enable(bullet, startPosition,targetPosition);

        Simulate(curentBullet);

        curentBullet.InitStop(Stop);
    }

    public override void Update(float delta)
    {
        if (delta <= 0)
            throw new InvalidOperationException();

        foreach (Bullet bullet in Entities)
            bullet.Update(delta);
    }

    public void Stop(Bullet bullet)
    {
        _spawner.Disable(bullet);
        bullet.OnEnd();
    }

    private void Distroy(IEnumerable<Bullet> bullets)
    {
        _spawner.Distroy(bullets);
        OnDistroy -= Distroy;
    }
}