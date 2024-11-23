using UnityEngine;

public class StateIdle : State
{
    protected override void EnterAfterAddon() => Complete();
}