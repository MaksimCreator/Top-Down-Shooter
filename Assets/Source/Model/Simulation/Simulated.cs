using System;
using System.Linq;
using System.Collections.Generic;

public abstract class Simulated<T> : ISimulated where T : class
{
    private HashSet<EntityManagerSimulated<T>> _entitys;

    protected event Action<IEnumerable<T>> OnDistroy;

    protected bool isActive { get; private set; } = true;
    protected IEnumerable<T> Entitys => _entitys.Select(Manager => Manager.Entitys);

    public virtual void StopSimulate()
    => isActive = false;

    public virtual void StartSimulate()
    => isActive = true;

    public void Update(float delta)
    {
        if (delta <= 0)
            throw new InvalidOperationException();

        onUpdate(delta);
    }

    public void AllStop()
    {
        OnDistroy.Invoke(Entitys);
        _entitys.Clear();
    }

    protected void TryAddEntity(EntityManagerSimulated<T> Entity)
    => _entitys.Add(Entity);
   
    protected abstract void onUpdate(float delta);

    protected bool IsUpdate(float delta)
    {
        if (delta <= 0)
            throw new InvalidOperationException();

        return isActive;
    }
}