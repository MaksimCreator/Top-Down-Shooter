public sealed class PlayerHealth : Health
{
    private bool _isInvulnerability = false;

    public PlayerHealth(int cutrentHealth, Entity model) : base(cutrentHealth, model)
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
            base.Death();
    }
}
