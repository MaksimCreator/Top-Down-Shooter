using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ViewFactroy<T> : MonoBehaviour,IService where T : class
{
    [SerializeField] private EntryPointGamplay _entryPointGamplay;

    private Dictionary<T, GameObject> _views = new();
    private PhysicsRouter _router;

    public void Creat(T prefab, Vector3 position,Quaternion rotation,Action<T,GameObject> action = null,Transform parent = null,bool IsPhysics = true)
    {
        GameObject model = Instantiate(GetTemplay(prefab),position,GetRotation(rotation));
        _views.Add(prefab, model);

        if (_router == null)
            _router = _entryPointGamplay.ServiceLocator.GetSevice<PhysicsRouter>();

        if (model.TryGetComponent(out PhysicsEventBroadcaster broadcaster) && IsPhysics)
            broadcaster.Init(prefab, _router);

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