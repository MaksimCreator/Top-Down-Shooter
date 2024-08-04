public sealed class Player : Entity
{
    private readonly PlayerHealth _health;

    public Player(PlayerHealth health,float speed) : base(speed)
    {
        _health = health;
    }

    public void Death()
    => _health.Death();

    public void StartGainAcceleration(float coolDown)
    {
        float curentSpeed = Speed;
        Speed *= Constant.GainAccaleration;
        Timer.StartTimer(coolDown, () => Speed = curentSpeed);
    }

    public void EnterSlowDown()
    => Speed *= Constant.ZoneEnterSlowDown;

    public void ExitSlowDown()
    => Speed *= Constant.ZoneExitSlowDown;
}