using UnityEngine;

[CreateAssetMenu(fileName = "NewStateSearchPlaceInterestConfig", menuName = "StateMachine/State/StateSearchPlaceInterestConfig")]
public class StateSearchPlaceInterestConfig : StateConfig
{
    public override State CreateState(Character character) => new StateSearchPlaceInterest(Id, character, TimeSecondsWaitUpdate);
}