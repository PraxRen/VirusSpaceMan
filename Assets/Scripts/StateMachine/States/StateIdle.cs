using UnityEngine;

[CreateAssetMenu(fileName = "NewStateIdle", menuName = "StateMachine/StateIdle")]
public class StateIdle : State
{
    protected override void EnterAfterAddon() => Complete();
}