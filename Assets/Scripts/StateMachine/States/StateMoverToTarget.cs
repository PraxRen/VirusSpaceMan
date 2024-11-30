using System;
using UnityEngine;
using UnityEngine.AI;

public class StateMoverToTarget : State
{
    private readonly Mover _mover;
     
    public StateMoverToTarget(string id, AICharacter character, float timeSecondsWaitHandle) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _mover) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(Mover)}\" required for operation \"{GetType().Name}\".");
    }

    public override void Update()
    {
        Vector2 direction = Navigation.CalculateDirectionVector2(Character.NavMeshAgent, Character.Transform, Character.MoveTracker.Position);
        _mover.Move(direction);
        _mover.LookAtDirection(direction);
    }

    protected override void EnterAfterAddon()
    {
        Navigation.ResetNavMeshAgent(Character.NavMeshAgent);
    }

    protected override void ExitAfterAddon()
    {
        Navigation.ResetNavMeshAgent(Character.NavMeshAgent);
        _mover.Cancel();
    }

    protected override bool CanUpdateAddon()
    {
        return _mover.CanMove();
    }
}