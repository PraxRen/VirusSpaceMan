using UnityEngine;

[CreateAssetMenu(fileName = "NewStateSearchPlaceInterestConfig", menuName = "StateMachine/States/StateSearchPlaceInterestConfig")]
public class StateSearchPlaceInterestConfig : StateConfig
{
    [Min(0f)][SerializeField] float _timeDelayComplete;

    public override State CreateState(Character character) => new StateSearchPlaceInterest(Id, character, TimeSecondsWaitUpdate, _timeDelayComplete);
}