public abstract class Gain
{
    protected readonly float Cooldown;

    protected Gain(float cooldown)
    {
        Cooldown = cooldown;
    }

    public abstract void Active(Player plaery);
}
