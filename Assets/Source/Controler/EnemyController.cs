using Model;
using System.Collections.Generic;
using UnityEngine;

namespace GameControllers
{
    public class EnemyController : IDeltaUpdatable
    {
        private readonly EnemySimulation _enemySimulation;
        private readonly Dictionary<IEnemyData, IUpdatable> _pairEnemyFsm;

        public ISimulated Simulated => _enemySimulation;

        public EnemyController(ServiceLocator serviceLocator, Wallet walletPlayer, Camera camera, Player player)
        {
            _enemySimulation = new EnemySimulation(AddNewEnemy,serviceLocator,walletPlayer,camera,player);
        }

        public void Update(float delta)
        {
            foreach (var fsm in _pairEnemyFsm.Values)
                fsm.Update();

            _enemySimulation.Update(delta);
        }

        private void AddNewEnemy(IEnemyData enemy)
        {
            if (_pairEnemyFsm.TryGetValue(enemy, out IUpdatable fsmUpdatable) == false)
            {
                Fsm fsm = new Fsm();

                fsm.BindState(new EnemyMovemingIdelState(enemy, fsm))
                    .BindState(new EnemyMovemengState(enemy,fsm));

                fsm.SetState<EnemyMovemingIdelState>();
                _pairEnemyFsm.Add(enemy, fsm);
            }
        }
    }
}