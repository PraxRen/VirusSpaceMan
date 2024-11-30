using UnityEngine;

public class TransitionTimerConfig : TransitionConfig
{
    [SerializeField] private float _timeTimer;

    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState)
    {
        return new TransitionTimer(character, currentState, targetState, _timeTimer);
    }
}