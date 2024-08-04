using System;

public abstract class Health
{
    private const int _minHealth = 0;

    private readonly Entity _model;
    
    private int _curentHealth;

    public event Action<Entity> onDeath;

    protected Health(int cutrentHealth, Entity model)
    {
        _curentHealth = cutrentHealth;
        _model = model;
    }

    protected void TakeDamage(int damage)
    {
        if (_curentHealth <= 0)
            throw new InvalidOperationException(nameof(damage));

        if(damage <= 0)
            throw new InvalidOperationException(nameof(damage)) ;

        if (_curentHealth - damage <= 0)
            Death();
        else
        {
            _curentHealth -= damage;
        }
    }

    protected void Death()
    {
        _curentHealth = 0;
        onDeath?.Invoke(_model);
    }

}
