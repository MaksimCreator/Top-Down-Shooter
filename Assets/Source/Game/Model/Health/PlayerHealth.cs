using System;

public sealed class PlayerHealth : Health
{
    private const int Health = 100;

    private bool _isInvulnerability = false;

    public event Action onDeath;

    public PlayerHealth() : base(Health)
    {
    }

    public void SetInvulnerability(bool value)
    {
        if(_isInvulnerability != value)
            _isInvulnerability = value;
    }

    public void TakeDamage()
    {
        if (_isInvulnerability == false)
            TakeDamage(Health);
    }

    protected override void OnDeath()
    => onDeath?.Invoke();
}
