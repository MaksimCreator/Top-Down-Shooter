public sealed class Accaleration : Gain
{
    public Accaleration(float cooldown) : base(cooldown)
    {
    }

    public override void Active(Player player)
    => player.StartGainAcceleration(Cooldown);
}