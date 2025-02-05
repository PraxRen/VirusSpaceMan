using UnityEngine;
using UnityEngine.AI;

public abstract class AICharacter : Character
{
    [SerializeField] private StateMachine _stateMachine;
    [SerializeField] private Trigger _triggerDamageable;

    public Trigger TriggerDamageable => _triggerDamageable;

    protected override void EnableAddon()
    {
        _triggerDamageable.enabled = true;
        _stateMachine.enabled = true;
    }

    protected override void DisableAddon()
    {
        _triggerDamageable.enabled = false;
        _stateMachine.enabled = false;
    }
}