using UnityEngine;

public class EnemyViewFactory : ViewFactroy<Enemy>
{
    [SerializeField] private GameObject _privateSoldierPrefab;
    [SerializeField] private GameObject _armoredSoldierPrefab;
    [SerializeField] private GameObject _nimbleSoldierPrefab;

    private EnemyVisiter _visiter;

    private void Awake()
    => _visiter = new EnemyVisiter(_privateSoldierPrefab,_armoredSoldierPrefab,_nimbleSoldierPrefab);

    protected override GameObject GetTemplay(Enemy enemy)
    {
        _visiter.Visit((dynamic)enemy);
        return _visiter.Prefab;
    }

    private class EnemyVisiter : IEnemyVisiter
    {
        private readonly GameObject _privateSoldierPrefab;
        private readonly GameObject _armoredSoldierPrefab;
        private readonly GameObject _nimbleSoldierPrefab;

        public GameObject Prefab { get; private set; }

        public EnemyVisiter(GameObject privateSoldierPrefab, GameObject armoredSoldierPrefab, GameObject nimbleSoldierPrefab)
        {
            _privateSoldierPrefab = privateSoldierPrefab;
            _armoredSoldierPrefab = armoredSoldierPrefab;
            _nimbleSoldierPrefab = nimbleSoldierPrefab;
        }

        public void Visit(PrivateSoldier visit)
        => Prefab = _privateSoldierPrefab;

        public void Visit(NimbleSoldier visit)
        => Prefab = _armoredSoldierPrefab;

        public void Visit(ArmoredSoldier visit)
        => Prefab = _nimbleSoldierPrefab;
    }
}