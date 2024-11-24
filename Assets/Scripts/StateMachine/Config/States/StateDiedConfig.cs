using UnityEngine;

[CreateAssetMenu(fileName = "NewStateDiedConfig", menuName = "StateMachine/State/StateDiedConfig")]
public class StateDiedConfig : StateConfig
{
    public override State CreateState(Character character) => new StateDied(Id, character, TimeSecondsWaitUpdate);
}