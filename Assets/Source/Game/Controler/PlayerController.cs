using System;
using UnityEngine;

public class PlayerController : ICursorDate
{
    private readonly ControleInputDate _inputService;
    private readonly Camera _mainCamera;
    private readonly IPlayerMovemeng _playerMovemeng;
    private readonly IUpdatebleFsm _fsmPlayerMovemeng;
    private readonly IUpdatebleFsm _fsmPlayerAttack;

    public Vector3 MousePosition => _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.nearClipPlane));

    public PlayerController(IInputService service, PlayerMovemeng move, Camera camera, Fsm fsmMovemeng, Fsm fsmAttack)
    {
        _inputService = new ControleInputDate(service);
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

        _playerMovemeng.Rotate(MousePosition);
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
