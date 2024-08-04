using UnityEngine;
using System;

public class GainViewFactory : ViewFactroy<Gain>
{
    [SerializeField] private GameObject _prefabAccaleration;
    [SerializeField] private GameObject _prefabInvulnerability;

    protected override GameObject GetTemplay(Gain prefab)
    {
        if (prefab is Accaleration)
            return _prefabAccaleration;
        else if (prefab is Invulnerability)
            return _prefabInvulnerability;

        throw new InvalidOperationException();
    }
}
