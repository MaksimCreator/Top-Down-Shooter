using System;
using System.Numerics;

public class PlayerController
{
    private readonly Fsm _fsmPlayer;
    private readonly PlayerMoveming _movemeng;

    public PlayerController(Fsm player)
    {
        _fsmPlayer = player;
    }

    public void Move(float delta,Vector2 direction)
    {
        if (direction == Vector2.Zero)
            return;

        if (delta <= 0)
            throw new InvalidOperationException(nameof(delta));

        float x = direction.X < 0 ? -1 : 1;
        float y = direction.Y < 0 ? -1 : 1;

        _movemeng.Move(x,y,delta);
    }

    public void Update()
    { 
        _fsmPlayer.Update();
    }
}
