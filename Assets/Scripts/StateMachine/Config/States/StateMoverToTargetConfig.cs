using UnityEngine;

[CreateAssetMenu(fileName = "NewStateMoverToTargetConfig", menuName = "StateMachine/States/StateMoverToTargetConfig")]
public class StateMoverToTargetConfig : StateConfig
{
    public override State CreateState(Character character) => new StateMoverToTarget(Id, character, TimeSecondsWaitUpdate);
}