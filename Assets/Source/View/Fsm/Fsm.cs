using System;
using System.Collections.Generic;

public class Fsm
{
    private FsmState _curentState;
    private Dictionary<Type, FsmState> _states = new();

    public bool IsState<T>() where T : FsmState
    => _curentState.GetType() == typeof(T);

    public void SetState<T>() where T : FsmState
    {
        var type = typeof(T);

        if (_curentState.GetType() == type)
            return;

        if (_states.TryGetValue(type, out var nextState))
        {
            _curentState?.Exit();
            _curentState = nextState;
            _curentState.Enter();
        }
        else
        {
            throw new InvalidOperationException();
        }

    }

    public Fsm BindState(FsmState state)
    {
        _states.TryAdd(state.GetType(), state);
        return this;
    }

    public void Update()
    => _curentState.Update();
}