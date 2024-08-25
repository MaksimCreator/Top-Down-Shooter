using UnityEngine;
using System;
using Model;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        private IInputControl _inputControl;
        private IDeltaUpdatable _bulletControler;
        private IDeltaUpdatable _playerController;
        private ISimulated _bulletSimulated;

        public void Activate(PlayerController playerController,BulletController bulletController,ServiceLocator locator)
        {
            _playerController = playerController;
            _bulletControler = bulletController;
            _bulletSimulated = bulletController.Simulated;
            _inputControl = locator.GetSevice<InputRouter>();
            enabled = true;
        }

        public void Disable()
        {
            _inputControl.Enable();
            _bulletSimulated.StartSimulate();
        }

        public void Enable()
        {
            if (enabled == false)
                throw new InvalidOperationException();

            _inputControl.Disable();
            _bulletSimulated.StopSimulate();
        }

        public void AllStop()
        {
            Disable();
            _bulletSimulated.AllStop();
            enabled = false;
        }

        private void Update()
        { 
            _bulletControler.Update(Time.deltaTime);
            _playerController.Update(Time.deltaTime);
        }
    }
}