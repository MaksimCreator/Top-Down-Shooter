using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerBullet
{
    private readonly BulletViewFactory _factory;
    private BulletVisiter _bulletVisiter;

    public SpawnerBullet(BulletViewFactory factory)
    {
        _factory = factory;
        _bulletVisiter = new BulletVisiter(Instantiat);
    }

    public Bullet Enable(Bullet bullet, Transform StartPosition)
    {
        _bulletVisiter.Visit((dynamic)bullet);
        (Bullet,GameObject) pair = _bulletVisiter.CurentPoolObject.Enable(bullet,StartPosition);
        pair.Item2.transform.Rotate(Input.mousePosition);

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

    private void Instantiat(Bullet bullet,Transform StartPosition)
    => _factory.Creat(bullet, StartPosition,_bulletVisiter.CurentPoolObject.AddObject);

    private class BulletVisiter : IBulletVisiter
    {
        private PoolObject<Bullet> _explosiveBullet;
        private PoolObject<Bullet> _defoltBullet;

        private Action<Bullet, Transform> _instantiate;

        public PoolObject<Bullet> CurentPoolObject;

        public BulletVisiter(Action<Bullet, Transform> instantiate)
        {
            if (_explosiveBullet != null)
                _explosiveBullet.onInstantiat -= _instantiate;
            if(_defoltBullet != null)
                _defoltBullet.onInstantiat -= _instantiate;

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