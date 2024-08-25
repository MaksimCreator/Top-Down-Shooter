using Managers;
using Model;

namespace State
{
    public class GuiInatelisateState : FsmState
    {
        private readonly EndGameView _endGameView;
        private readonly GamplayMenuView _gamplayMenuView;
        private readonly ExitMenuView _exitMenuView;
        private readonly GameManager _gameManager;
        private readonly PlayerDate _playerDate;
        private readonly PlayerHealth _playerHealth;
        private readonly Wallet _wallet;

        public GuiInatelisateState(Wallet wallet,PlayerHealth playerHealth,PlayerDate playerDate,GameManager gameManager,GamplayMenuView gamplayMenuView,ExitMenuView exitMenuView,EndGameView endGameView,Fsm fsm) : base(fsm)
        {
            _endGameView = endGameView;
            _gamplayMenuView = gamplayMenuView;
            _exitMenuView = exitMenuView;
            _gameManager = gameManager;
            _playerDate = playerDate;
            _playerHealth = playerHealth;
            _wallet = wallet;
        }

        public override void Enter()
        {
            EnterGamplayMenu();
            _playerHealth.onDeath += OnEnd;
            Fsm.SetState<PhysicsRoutingState>();
        }

        private void OnEnd()
        {
            ExitGamplayMenu();
            EnterEndGame();
            _playerHealth.onDeath -= OnEnd;
        }

        private void EnterGamplayMenu()
        {
            _gameManager.Enable();
            _gamplayMenuView.Init(_wallet, () => EnterExitMenu());
        }

        private void EnterExitMenu()
        {
            _gameManager.Disable();
            _exitMenuView.Enter(() =>
            {
                ExitExitMenuView();
                EnterGamplayMenu();
            }, _wallet.Score);
        }

        private void EnterEndGame()
        {
            _endGameView.Enter(_wallet.Score);
            _gameManager.AllStop();
        }

        private void ExitGamplayMenu()
        => _gamplayMenuView.Disable();

        private void ExitExitMenuView()
        => _exitMenuView.Exit();
    }
}