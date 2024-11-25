using UnityEngine;

[CreateAssetMenu(fileName = "NewStateDeathConfig", menuName = "StateMachine/States/StateDeathConfig")]
public class StateDeathConfig : StateConfig
{
    public override State CreateState(Character character) => new StateDeath(Id, character, TimeSecondsWaitUpdate);
}