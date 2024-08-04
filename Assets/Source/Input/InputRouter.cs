using System.Numerics;
using UnityEngine.InputSystem;

public class InputRouter
{
    private readonly InputPlayer _input;
    private readonly PlayerController _controller;

    public InputRouter(PlayerController controller)
    {
        _controller = controller;
    }

    public void Enable()
    => _input.Enable();

    public void Disable()
    => _input.Disable();

    public void Update(float delta)
    {
        if (IsPerfomedMovemeng())
            _controller.Move(delta,_input.Player.Movemeng.ReadValue<Vector2>());

        _controller.Update();
    }

    private bool IsPerfomedMovemeng()
    => _input.Player.Movemeng.phase == InputActionPhase.Performed;
}
