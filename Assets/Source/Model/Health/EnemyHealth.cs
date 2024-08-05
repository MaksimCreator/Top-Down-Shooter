using System;

public sealed class EnemyHealth : Health
{
    private readonly Entity _entity;

    public event Action<Entity> onDeath;

    public EnemyHealth(int cutrentHealth, Entity model) : base(cutrentHealth)
    {
    }

    public new void TakeDamage(int damage)
    => base.TakeDamage(damage);

    protected override void OnDeath()
    => onDeath?.Invoke(_entity);
}
