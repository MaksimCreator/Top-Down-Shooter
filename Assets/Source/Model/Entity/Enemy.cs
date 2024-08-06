public class Enemy : Entity
{
    private EnemyHealth _health;

    public Enemy(float speed) : base(speed)
    {
    }

    public void BindHealth(EnemyHealth health)
    => _health = health;

    public void TakeDamage(int damage)
    => _health.TakeDamage(damage);
}