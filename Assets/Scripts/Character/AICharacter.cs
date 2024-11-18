using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public abstract class AICharacter : Character
{
    private StateMachine _stateMachine;

    protected override void AwakeAddon()
    {
        _stateMachine = GetComponent<StateMachine>();
    }

    protected override void EnableAddon()
    {
        _stateMachine.enabled = true;
    }

    protected override void DisableAddon()
    {
        _stateMachine.enabled = false;
    }
}