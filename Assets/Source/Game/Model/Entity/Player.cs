using UnityEngine;

public sealed class Player : Entity
{
    private readonly PlayerHealth _health;

    public Player(PlayerHealth health,Transform transfomPlayer,float speed) : base(speed)
    {
        _health = health;

        TryAddTransformEntity(transfomPlayer);
    }

    public void TryDeath()
    => _health.TakeDamage();

    public void Death()
    {
        _health.SetInvulnerability(false);
        _health.TakeDamage();
    }

    public void StartGainAcceleration(float coolDown)
    {
        Speed *= Constant.GainEnterAccaleration;
        Timers.StartTimer(coolDown, () => Speed /= Constant.GainEnterAccaleration);
    }

    public void StartGainInvulnerability(float coolDown)
    { 
        _health.SetInvulnerability(true);
        Timers.StartTimer(coolDown, () => _health.SetInvulnerability(false));
    } 

    public void EnterSlowDown()
    => Speed *= Constant.ZoneEnterSlowDown;

    public void ExitSlowDown()
    => Speed *= Constant.ZoneExitSlowDown;
}