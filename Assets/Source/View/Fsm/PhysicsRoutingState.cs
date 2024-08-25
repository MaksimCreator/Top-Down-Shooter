using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace State
{
    public class PhysicsRoutingState : FsmState
    {
        private readonly MonoBehaviour _monobex;
        private readonly PhysicsRouter _router;
        private readonly List<PhysicsEventBroadcaster> _walls;
        private readonly PhysicsEventBroadcaster _player;

        public PhysicsRoutingState(MonoBehaviour monobex,List<PhysicsEventBroadcaster> walls,PhysicsEventBroadcaster player,PhysicsRouter router,Fsm fsm) : base(fsm)
        {
            _monobex = monobex;
            _walls = walls;
            _player = player;
            _router = router;
        }

        public override void Enter()
        {
            _player.Init(new Obstacle(), _router);
            foreach (var wall in _walls)
                wall.Init(new Obstacle(), _router);

            _monobex.StartCoroutine(GetRouterSteper());
        }

        public override void Exit()
        => _monobex.StopCoroutine(GetRouterSteper());

        private IEnumerator GetRouterSteper()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                _router.Step();
            }
        }
    }
}
