using UnityEngine;

[CreateAssetMenu(fileName = "NewStateIdleConfig", menuName = "StateMachine/States/StateIdleConfig")]
public class StateIdleConfig : StateConfig
{
    public override State CreateState(AICharacter character) => new StateIdle(Id, character, TimeSecondsWaitUpdate);
}