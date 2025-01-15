using UnityEngine;

[CreateAssetMenu(fileName = "NewTransitionDeathConfig", menuName = "StateMachine/Transitions/TransitionDeathConfig")]
public class TransitionDeathConfig : TransitionConfig
{
    protected override Transition CreatTransitionAddon(Character character, State currentState, State targetState)
    {
        return new TransitionDeath(character, currentState, targetState);
    }
}