using GameControllers;
using UnityEngine;
using System;

namespace Managers
{
    public class EnemyManager : MonoBehaviour
    {
        private IDeltaUpdatable _controller;
        private ISimulated _simulation;

        public void Activate(EnemyController contoller)
        {
            _controller = contoller;
            _simulation = contoller.Simulated;
        }

        public void Enable()
        {
            if (enabled == false)
                throw new InvalidOperationException();

            _simulation.StopSimulate();
        }

        public void Disable()
        => _simulation.StopSimulate();

        public void AllStop()
        {
            _simulation.AllStop();
            enabled = false;
        }

        private void Update()
        => _controller.Update(Time.deltaTime);
    }
}
namespace GameControllers
{
}