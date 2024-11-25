using UnityEngine;

[CreateAssetMenu(fileName = "NewStateSearchPlaceInterestConfig", menuName = "StateMachine/States/StateSearchPlaceInterestConfig")]
public class StateSearchPlaceInterestConfig : StateConfig
{
    public override State CreateState(Character character) => new StateSearchPlaceInterest(Id, character, TimeSecondsWaitUpdate);
}