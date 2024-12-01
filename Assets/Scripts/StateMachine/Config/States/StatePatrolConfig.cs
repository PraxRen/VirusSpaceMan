using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatePatrolConfig", menuName = "StateMachine/States/StatePatrolConfig")]
public class StatePatrolConfig : StateConfig
{
    [SerializeField] private ModeMover _modeMover;

    public override State CreateState(AICharacter character) => new StatePatrol(Id, character, TimeSecondsWaitUpdate, _modeMover);
}