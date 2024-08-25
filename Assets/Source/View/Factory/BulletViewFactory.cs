using UnityEngine;

public class BulletViewFactory : ViewFactroy<Bullet>
{
    [SerializeField] private GameObject _defoltBullet;
    [SerializeField] private GameObject _explosiveBullet;

    private BulletVisiter _visiter;

    private void Awake()
    => _visiter = new BulletVisiter(_defoltBullet, _explosiveBullet);

    protected override GameObject GetTemplay(Bullet prefab)
    {
        _visiter.Visit((dynamic)(prefab));
        return _visiter.Prefab;
    }

    private class BulletVisiter : IBulletVisiter
    {
        private readonly GameObject _defoltBullet;
        private readonly GameObject _explosiveBullet;

        public GameObject Prefab { get; private set; }

        public BulletVisiter(GameObject defoltBullet, GameObject explosiveBullet)
        {
            _defoltBullet = defoltBullet;
            _explosiveBullet = explosiveBullet;
        }

        public void Visit(DefoltBullet bullet)
        => Prefab = _defoltBullet;

        public void Visit(ExplosiveBullet bullet)
        => Prefab = _explosiveBullet;

        public void Visit(ShotgunBullet bullet)
        => Prefab = _defoltBullet;
    }
}
