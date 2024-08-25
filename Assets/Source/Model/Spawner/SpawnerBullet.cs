using Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerBullet
{
    private readonly BulletViewFactory _factory;
    private BulletVisiter _bulletVisiter;

    public SpawnerBullet(ServiceLocator locator)
    {
        _factory = locator.GetSevice<BulletViewFactory>();
        _bulletVisiter = new BulletVisiter(Instantiat);
    }

    public Bullet Enable(Bullet bullet, Transform StartPosition,Vector3 targetPosition)
    {
        _bulletVisiter.Visit((dynamic)bullet);
        (Bullet,GameObject) pair = _bulletVisiter.CurentPoolObject.Enable(bullet,StartPosition.position,StartPosition.rotation);
        pair.Item2.transform.LookAt(targetPosition);

        if (bullet is ShotgunBullet shotgunBullet)
            pair.Item2.transform.rotation = Quaternion.Euler(0, Random.Range(-shotgunBullet.AngelBullet, shotgunBullet.AngelBullet),0);

        pair.Item1.StartMovemeng(pair.Item2);
        return pair.Item1;
    }

    public void Disable(Bullet bullet)
    => _bulletVisiter.CurentPoolObject.Disable(bullet);

    public void Distroy(IEnumerable<Bullet> allBullet)
    {
        foreach (Bullet bullet in allBullet)
            _factory.Destroy(bullet);

        _bulletVisiter = new BulletVisiter(Instantiat);
    }

    private void Instantiat(Bullet bullet,Vector3 position,Quaternion rotation)
    => _factory.Creat(bullet, position,rotation,_bulletVisiter.CurentPoolObject.AddObject);

    private class BulletVisiter : IBulletVisiter
    {
        private PoolObject<Bullet> _explosiveBullet;
        private PoolObject<Bullet> _defoltBullet;

        public PoolObject<Bullet> CurentPoolObject;

        public BulletVisiter(Action<Bullet, Vector3, Quaternion> instantiate)
        {
            _explosiveBullet = new();
            _defoltBullet = new();

            _explosiveBullet.onInstantiat += instantiate;
            _defoltBullet.onInstantiat += instantiate;
        }

        public void Visit(DefoltBullet bullet)
        => CurentPoolObject = _defoltBullet;

        public void Visit(ShotgunBullet bullet)
        => CurentPoolObject = _defoltBullet;

        public void Visit(ExplosiveBullet bullet)
        => CurentPoolObject = _explosiveBullet;
    }
}