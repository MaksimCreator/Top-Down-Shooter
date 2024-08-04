using UnityEngine;

public class WeaponViewFactory : ViewFactroy<Weapon>
{
    [SerializeField] private GameObject _automaton;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _shotgun;
    [SerializeField] private GameObject _grenadeLauncher;

    private WeaponVisit _visit;

    private void Awake()
    => _visit = new WeaponVisit(_automaton,_gun,_shotgun,_grenadeLauncher);

    protected override GameObject GetTemplay(Weapon prefab)
    {
        _visit.Visit((dynamic)(prefab));
        return _visit.Prefab;
    }

    private class WeaponVisit : IWeaponVisiter
    {
        private readonly GameObject _automaton;
        private readonly GameObject _gun;
        private readonly GameObject _shotgun;
        private readonly GameObject _grenadeLauncher;

        public GameObject Prefab { get; private set; }

        public WeaponVisit(GameObject automaton, GameObject gun, GameObject shotgun, GameObject grenadeLauncher)
        {
            _automaton = automaton;
            _gun = gun;
            _shotgun = shotgun;
            _grenadeLauncher = grenadeLauncher;
        }

        public void Visit(Automaton visit)
        => Prefab = _automaton;

        public void Visit(Gun visit)
        => Prefab = _gun;

        public void Visit(Shotgun visit)
        => Prefab = _shotgun;

        public void Visit(GrenadeLauncher visit)
        => Prefab = _grenadeLauncher;
    }
}
