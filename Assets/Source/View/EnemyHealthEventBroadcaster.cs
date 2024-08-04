using UnityEngine;

public class EnemyHealthEventBroadcaster : MonoBehaviour
{
    private EnemyHealth _health;

    public void Init(EnemyHealth health)
    => _health = health;

    public void TakeDamage(int damage)
    => _health.TakeDamage(damage);
}