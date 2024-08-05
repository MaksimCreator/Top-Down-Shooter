using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ViewFactroy<T> : MonoBehaviour
{
    [SerializeField] private PhysicsCompositRoot _physicsCompositRoot;

    private Dictionary<T, GameObject> _views = new();

    public void Creat(T prefab, Transform transformWorldSpace,Action<T,GameObject> action = null,bool isParent = false,bool IsPhysics = true)
    {
        GameObject model = Instantiate(GetTemplay(prefab),transformWorldSpace);
        _views.Add(prefab, model);

        if (action != null)
            action(prefab, model);

        if (model.TryGetComponent(out PhysicsEventBroadcaster broadcaster) && IsPhysics)
            broadcaster.Init(prefab, _physicsCompositRoot.Router);
        
        if(isParent)
            model.transform.parent = transformWorldSpace;
    }

    public void Destroy(T prefab)
    {
        GameObject model = _views[prefab];
        _views.Remove(prefab);
        Destroy(model);
    }

    protected abstract GameObject GetTemplay(T prefab);
}
