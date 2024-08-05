using System.Collections.Generic;
using System;

public abstract class Simulated<T> : IUpdateble where T : class
{
    private HashSet<T> _allEntity = new();

    protected IEnumerable<T> Entities => _allEntity;

    protected event Action<IEnumerable<T>> OnDistroy;

    public abstract void Update(float delta);

    protected void Simulate(T Entity)
    => _allEntity.Add(Entity);

    public void AllStop()
    {
        OnDistroy.Invoke(Entities);
        _allEntity.Clear();
    }
    
}
