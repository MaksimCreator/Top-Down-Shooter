using UnityEngine;

public class CompositRootUI : CompositRoot
{
    [SerializeField] private PlayerCompositRoot _playerCompositRoot;
    [SerializeField] private ExitMenuView _exitMenuView;
    [SerializeField] private GamplayMenuView _gamplayMenuView;

    public override void Compos()
    => _gamplayMenuView.Init(_playerCompositRoot.Wallet, _exitMenuView.Enter);
}   
