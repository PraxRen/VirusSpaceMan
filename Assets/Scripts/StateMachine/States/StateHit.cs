using UnityEngine;

public class StateHit : State
{
    public StateHit(string id, AICharacter character, float timeSecondsWaitHandle) : base(id, character, timeSecondsWaitHandle) { }

    protected override void EnterAfterAddon() => Complete();
}