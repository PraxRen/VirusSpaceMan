using UnityEngine;
using System;

public abstract class TransitionConfig : ScriptableObject
{
    [SerializeField] private StateConfig _currentStateConfig;
    [SerializeField] private StateConfig _targetStateConfig;

    public StateConfig CurrentState => _currentStateConfig;
    public StateConfig TargetState => _targetStateConfig;

    public Transition CreatTransition(Character character, State currentState, State targetState)
    {
        if (_currentStateConfig.Id != currentState.Id)
            throw new InvalidOperationException();

        if (_targetStateConfig.Id != targetState.Id)
            throw new InvalidOperationException();

        return CreatTransitionAddon(character, currentState, targetState);
    }

    protected abstract Transition CreatTransitionAddon(Character character, State currentState, State targetState);
}