using UnityEngine;

[CreateAssetMenu(fileName = "NewStateSearchPatrolPathConfig", menuName = "StateMachine/States/StateSearchPatrolPathConfig")]
public class StateSearchPatrolPathConfig : StateConfig
{
    [Min(0f)][SerializeField] float _timeDelayComplete;

    public override State CreateState(AICharacter character) => new StateSearchPatrolPath(Id, character, TimeSecondsWaitUpdate, _timeDelayComplete);
}