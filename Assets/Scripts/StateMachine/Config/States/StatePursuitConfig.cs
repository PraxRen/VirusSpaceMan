using UnityEngine;

[CreateAssetMenu(fileName = "NewStatePursuitConfig", menuName = "StateMachine/States/StatePursuitConfig")]
public class StatePursuitConfig : StateConfig
{
    [SerializeField] private ModeMover _modeMover;

    public override State CreateState(AICharacter character) => new StatePursuit(Id, character, TimeSecondsWaitUpdate, _modeMover);
}