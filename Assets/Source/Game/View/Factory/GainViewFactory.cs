using UnityEngine;
using System;

public class GainViewFactory : ViewFactroy<Gain>
{
    [SerializeField] private GameObject _prefabAccaleration;
    [SerializeField] private GameObject _prefabInvulnerability;

    private GainVisiter _visiter;

    private void Awake()
    => _visiter = new GainVisiter(_prefabAccaleration,_prefabInvulnerability);

    protected override GameObject GetTemplay(Gain prefab)
    {
        _visiter.Visit((dynamic)prefab);
        return _visiter.Prefab;
    }

    private class GainVisiter : IGainVisiter
    {
        private readonly GameObject _prefabAccaleration;
        private readonly GameObject _prefabInvulnerability;

        public GameObject Prefab { get; private set; }

        public GainVisiter(GameObject prefabAccaleration, GameObject prefabInvulnerability)
        {
            _prefabAccaleration = prefabAccaleration;
            _prefabInvulnerability = prefabInvulnerability;
        }

        public void Visit(Accaleration visit)
        => Prefab = _prefabAccaleration;

        public void Visit(Invulnerability visit)
        => Prefab = _prefabInvulnerability;
    }
}