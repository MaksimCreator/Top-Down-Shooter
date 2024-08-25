using Model;
using System;
using UnityEngine;

public class PlayerController : IDeltaUpdatable
{
    private readonly ControleInputDate _inputService;
    private readonly Camera _mainCamera;
    private readonly IPlayerMovemeng _playerMovemeng;
    private readonly IUpdatable _fsmPlayerMovemeng;
    private readonly IUpdatable _fsmPlayerAttack;
    private readonly IMousePosition _mousePosition;

    public PlayerController(ServiceLocator locator, PlayerMovemeng move, Camera camera, Fsm fsmMovemeng, Fsm fsmAttack)
    {
        _inputService = new ControleInputDate(locator.GetSevice<InputRouter>());
        _mousePosition = locator.GetSevice<MouseDate>();
        _playerMovemeng = move;
        _mainCamera = camera;
        _fsmPlayerMovemeng = fsmMovemeng;
        _fsmPlayerAttack = fsmAttack;
    }

    public void Update(float delta)
    {
        if (delta <= 0)
            throw new InvalidOperationException(nameof(delta));

        if (_inputService.IsMove)
            _playerMovemeng.Move(_inputService.Direction.x,_inputService.Direction.y,delta);

        _playerMovemeng.Rotate(_mousePosition.mouseToWorldPosition);
        _playerMovemeng.Update(delta);

        _fsmPlayerAttack.Update();
        _fsmPlayerMovemeng.Update();
    }

    private class ControleInputDate
    {
        private readonly IInputService _inputService;

        public Vector2 Direction => _inputService.Direction;
        public bool IsMove => _inputService.IsPerfomedMovemeng();

        public ControleInputDate(IInputService service)
        {
            _inputService = service;
        }
    }
}
