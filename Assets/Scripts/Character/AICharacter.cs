using UnityEngine;

public abstract class AICharacter : Character
{
    [SerializeField] private StateMachine _stateMachine;

    protected override void EnableAddon()
    {
        _stateMachine.enabled = true;
    }

    protected override void DisableAddon()
    {
        _stateMachine.enabled = false;
    }
}