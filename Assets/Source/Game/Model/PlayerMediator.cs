using System;

public class PlayerMediator
{
    private readonly Wallet _playerWallet;
    private readonly Action _update;
    private readonly IDateService _service;

    public PlayerMediator(Wallet playerWallet, IDateService service, Action update)
    {
        _playerWallet = playerWallet;
        _service = service;
        _update = update;
    }

    public void Enable()
    => _playerWallet.OnUpdate += _update;

    public void Disable()
    => _playerWallet.OnUpdate -= _update;
}