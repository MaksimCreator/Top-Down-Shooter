public sealed class EnemyHealth : Health
{
    public EnemyHealth(int cutrentHealth, Entity model) : base(cutrentHealth, model)
    {
    }

    public new void TakeDamage(int damage)
    => base.TakeDamage(damage);
}
