using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ViewFactroy<T> : MonoBehaviour,IViewFactoryService<T> where T : class
{
    [SerializeField] private PhysicsCompositRoot _physicsCompositRoot;

    private Dictionary<T, GameObject> _views = new();

    public void Creat(T prefab, Vector3 position,Quaternion rotation,Action<T,GameObject> action = null,Transform parent = null,bool IsPhysics = true)
    {
        GameObject model = Instantiate(GetTemplay(prefab),position,GetRotation(rotation));
        _views.Add(prefab, model);

        if (model.TryGetComponent(out PhysicsEventBroadcaster broadcaster) && IsPhysics)
            broadcaster.Init(prefab, _physicsCompositRoot.Router);

        if (action != null)
            action(prefab, model);
        
        if(parent != null)
            model.transform.parent = parent;
    }

    public void Destroy(T prefab)
    {
        GameObject model = _views[prefab];
        _views.Remove(prefab);
        Destroy(model);
    }

    protected abstract GameObject GetTemplay(T prefab);

    protected virtual Quaternion GetRotation(Quaternion rotation)
    => rotation;
}