using System.Numerics;
using UnityEngine.InputSystem;

public class InputRouter
{
    private readonly InputPlayer _input;
    private readonly PlayerController _controller;
    private readonly InputPlayerAttack _inputAttack;

    public InputRouter(PlayerController controller,InputPlayerAttack inputAttack)
    {
        _controller = controller;
        _inputAttack = inputAttack;
    }

    public void Enable()
    => _input.Enable();

    public void Disable()
    => _input.Disable();

    public void Update(float delta)
    {
        if (IsPerfomedMovemeng())
            _controller.Move(delta,_input.Player.Movemeng.ReadValue<Vector2>());

        _inputAttack.Update(IsPerfomedAttack());
        _controller.Update();
    }

    private bool IsPerfomedMovemeng()
    => _input.Player.Movemeng.phase == InputActionPhase.Performed;

    private bool IsPerfomedAttack()
    => _input.Player.Shoot.phase == InputActionPhase.Performed;
}
