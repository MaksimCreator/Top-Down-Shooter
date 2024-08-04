public sealed class Accaleration : Gain
{
    public readonly Player _player;

    public Accaleration(Player player,float cooldown) : base(cooldown)
    {
        _player = player;
    }

    public override void Active()
    => _player.StartGainAcceleration(Cooldown);
}