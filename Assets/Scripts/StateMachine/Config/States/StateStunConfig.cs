using UnityEngine;

[CreateAssetMenu(fileName = "NewStateStunConfig", menuName = "StateMachine/States/StateStunConfig")]
public class StateStunConfig : StateConfig
{
    public override State CreateState(AICharacter character) => new StateStun(Id, character, TimeSecondsWaitUpdate);
}