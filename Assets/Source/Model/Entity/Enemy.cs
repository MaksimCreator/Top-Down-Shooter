public class Enemy : Entity
{
    private readonly EnemyHealth _health;

    public Enemy(EnemyHealth health, float speed) : base(speed)
    {
        _health = health;
    }

    public void TakeDamage(int damage)
    => _health.TakeDamage(damage);
}