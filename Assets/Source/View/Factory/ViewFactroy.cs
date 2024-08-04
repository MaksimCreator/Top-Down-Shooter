using System.Collections.Generic;
using UnityEngine;

public abstract class ViewFactroy<T> : MonoBehaviour
{
    [SerializeField] private PhysicsCompositRoot _physicsCompositRoot;

    private Dictionary<T, GameObject> _views = new();

    public void Creat(T prefab, Transform transformWorldSpace,bool isParent = false)
    {
        GameObject model = Instantiate(GetTemplay(prefab),transformWorldSpace.position, GetQuaternion(transformWorldSpace));
        _views.Add(prefab, model);
        
        if (model.TryGetComponent(out PhysicsEventBroadcaster broadcaster))
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

    protected virtual Quaternion GetQuaternion(Transform transformWorldSpace)
    => transformWorldSpace.rotation;
}
public class PlayerCompositRoot : CompositRoot
{
    [SerializeField] PlayerConffig _conffig;

    public WeaponPresenter Presenter { get; private set; }
    public Inventary Inevntary { get; private set; }

    public override void Init()
    {

    }
}