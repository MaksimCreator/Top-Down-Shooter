using UnityEngine;
using System;

public class ZoneViewFactory : ViewFactroy<Zone>
{
    [SerializeField] private GameObject _slowDownZone;
    [SerializeField] private GameObject _deathZone;

    protected override GameObject GetTemplay(Zone prefab)
    {
        if (prefab is SlowDownZone)
            return _slowDownZone;
        else if (prefab is DeadZone)
            return _deathZone;

        throw new InvalidOperationException();
    }
}
