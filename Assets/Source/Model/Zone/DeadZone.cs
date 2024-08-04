using System;

public class DeadZone : Zone
{
    protected override void EnterAction(Player player)
    => player.Death();

    protected override void ExitAction(Player player)
    => throw new InvalidOperationException();
}
