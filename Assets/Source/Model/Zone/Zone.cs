// В текущем коде можно было бы Zone and Gain сделать одной сущностью но в будущем можно сделать входные параметры для Zone  Entity а для Gain  Player
public abstract class Zone
{
    private bool _isAction = false;

    public void Active(Player player)
    {
        if (_isAction == false)
        {
            _isAction = true;
            EnterAction(player);
        }
        else
        {
            _isAction = false;
            ExitAction(player);
        }
    }

    protected abstract void EnterAction(Player player);
    protected abstract void ExitAction(Player player);
}