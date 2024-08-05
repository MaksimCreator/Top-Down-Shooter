using System;

public sealed class PlayerHealth : Health
{
    private bool _isInvulnerability = false;

    public event Action onDeath;

    public PlayerHealth(int cutrentHealth) : base(cutrentHealth)
    {
    }

    public void SetInvulnerability(bool value)
    {
        if(_isInvulnerability != value)
            _isInvulnerability = value;
    }

    public new void Death()
    {
        if (_isInvulnerability == false)
            OnDeath();
    }

    protected override void OnDeath()
    => onDeath?.Invoke();
}
