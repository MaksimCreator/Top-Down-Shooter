using Model;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputRouter : IInputService
{
    private readonly InputPlayer _input = new();

    public Vector2 Direction => _input.Player.Movemeng.ReadValue<Vector2>();

    public void Enable()
    => _input.Enable();

    public void Disable()
    => _input.Disable();

    public bool IsPerfomedMovemeng()
    => _input.Player.Movemeng.phase == InputActionPhase.Performed;

    public bool IsPerfomedAttack()
    => _input.Player.Shoot.phase == InputActionPhase.Performed;
}