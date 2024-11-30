using UnityEngine;

public class StateIdle : State
{
    public StateIdle(string id, AICharacter character, float timeSecondsWaitHandle) : base(id, character, timeSecondsWaitHandle) { }

    protected override void EnterAfterAddon() => Complete();
}