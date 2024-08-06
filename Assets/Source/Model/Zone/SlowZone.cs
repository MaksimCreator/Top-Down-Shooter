public class SlowZone : Zone
{
    protected override void EnterAction(Player player)
    => player.EnterSlowDown();

    protected override void ExitAction(Player player)
    => player.ExitSlowDown();
}
