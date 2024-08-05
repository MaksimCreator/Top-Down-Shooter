using System;

public abstract class Health
{
    private const int _minHealth = 0;

    private int _curentHealth;

    protected Health(int cutrentHealth)
    {
        _curentHealth = cutrentHealth;
    }

    protected void TakeDamage(int damage)
    {
        if (_curentHealth <= 0)
            throw new InvalidOperationException(nameof(damage));

        if (damage <= 0)
            throw new InvalidOperationException(nameof(damage));

        if (_curentHealth - damage <= 0)
            Death();
        else
            _curentHealth -= damage;
    }

    protected void Death()
    {
        _curentHealth = 0;
        OnDeath();
    }

    protected abstract void OnDeath();
}
