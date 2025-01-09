using UnityEngine;
using UnityEngine.AI;

public abstract class AICharacter : Character
{
    [SerializeField] private StateMachine _stateMachine;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Trigger _triggerDamageable;

    public Trigger TriggerDamageable => _triggerDamageable;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    protected override void AwakeAddon()
    {
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
    }

    protected override void EnableAddon()
    {
        _navMeshAgent.enabled = true;
        _stateMachine.enabled = true;
        _triggerDamageable.enabled = true;
    }

    protected override void DisableAddon()
    {
        _navMeshAgent.enabled = false;
        _stateMachine.enabled = false;
        _triggerDamageable.enabled = false;
    }
}