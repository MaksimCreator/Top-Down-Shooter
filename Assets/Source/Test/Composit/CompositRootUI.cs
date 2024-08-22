using UnityEngine;

public class CompositRootUI : CompositRoot
{
    [SerializeField] private PlayerCompositRoot _playerCompositRoot;
    [SerializeField] private CompositRootSimulated _playerSimulated;
    [SerializeField] private ExitMenuView _exitMenuView;
    [SerializeField] private GamplayMenuView _gamplayMenuView;

    public override void Compos()
    { 
        _gamplayMenuView.Init(_playerCompositRoot.Wallet, (action,score) =>
        {
            _playerSimulated.StopSimulated();
            _playerCompositRoot.OnDisable();
            _exitMenuView.Enter(() =>
            {
                _playerSimulated.StartSimulated();
                _playerCompositRoot.OnEnable();
                action();
            },score);
        });
    }
}   
