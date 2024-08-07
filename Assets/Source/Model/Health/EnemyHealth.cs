using System;

public sealed class EnemyHealth : Health
{
    private readonly Enemy _entity;

    public event Action<Enemy> onDeath;

    public EnemyHealth(int cutrentHealth, Enemy model) : base(cutrentHealth)
    {
    }

    public new void TakeDamage(int damage)
    => base.TakeDamage(damage);

    protected override void OnDeath()
    { 
        onDeath?.Invoke(_entity);
        
    }
}
