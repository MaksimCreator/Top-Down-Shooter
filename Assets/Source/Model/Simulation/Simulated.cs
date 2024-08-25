using System;
using System.Collections.Generic;

public abstract class Simulated<T1, T2> : ISimulated where T2 : class
{
    private readonly Dictionary<T1, T2> _dataEntityPairs = new();
    private readonly HashSet<T2> _entitys = new();
    private bool _isActive = true;

    protected event Action<IEnumerable<T2>> OnDistroy;

    protected IEnumerable<T2> Entitys => _entitys;

    public virtual void StopSimulate()
    => _isActive = false;

    public virtual void StartSimulate()
    => _isActive = true;

    public void Update(float delta)
    {
        if (delta <= 0)
            throw new InvalidOperationException();

        if (_isActive == false)
            return;

        onUpdate(delta);
    }

    public void AllStop()
    {
        OnDistroy.Invoke(_entitys);
        _entitys.Clear();
    }

    protected void TryAddEntity(T1 entityData, T2 entity)
    {
        if(_entitys.Add(entity))
            _dataEntityPairs.Add(entityData, entity);
    }
   
    protected T2 GetEntity(T1 interfaceEntity)
    => _dataEntityPairs[interfaceEntity];

    protected bool CanSimulated()
    => _isActive;

    protected abstract void onUpdate(float delta);
}