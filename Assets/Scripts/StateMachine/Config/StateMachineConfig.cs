using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStateMachineConfig", menuName = "StateMachine/StateMachineConfig")]
public class StateMachineConfig : ScriptableObject
{
    [SerializeField] private StateConfig[] _stateConfigs;
    [SerializeField] private TransitionConfig[] _transitionConfigs;

    public IEnumerable<State> CreatStates(Character character)
    {
        Dictionary<string, State> result = new Dictionary<string, State>();

        foreach (StateConfig stateConfig in _stateConfigs)
        {
            result.Add(stateConfig.Id, stateConfig.CreateState(character));
        }

        foreach (TransitionConfig transitionConfig in _transitionConfigs)
        {
            if (result.TryGetValue(transitionConfig.CurrentState.Id, out State currentState) == false)
                throw new InvalidOperationException();

            if (result.TryGetValue(transitionConfig.TargetState.Id, out State targetState) == false)
                throw new InvalidOperationException();

            Transition transition = transitionConfig.CreatTransition(character, currentState, targetState);
            currentState.AddTransition(transition);
        }

        return result.Values;
    }
}