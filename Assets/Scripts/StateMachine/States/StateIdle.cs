using UnityEngine;

public class StateIdle : State
{
    public StateIdle(string id, Character character, float timeSecondsWaitHandle) : base(id, character, timeSecondsWaitHandle) { }

    protected override void EnterAfterAddon() => Complete();
}