using UnityEngine;
using System;

[Serializable]
public class TransitionConfig
{
    [SerializeField] private TypeTransition _typeTransition;
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

        return _typeTransition switch
        {
            TypeTransition.Died => new TransitionDied(character, currentState, targetState),
            TypeTransition.StateComplete => new TransitionStateComplete(character, currentState, targetState),
            _ => throw new InvalidOperationException()
        };
    }
}