using UnityEngine;

[CreateAssetMenu(fileName = "NewStateHitConfig", menuName = "StateMachine/States/StateHitConfig")]
public class StateHitConfig : StateConfig
{
    public override State CreateState(AICharacter character) => new StateHit(Id, character, TimeSecondsWaitUpdate);
}