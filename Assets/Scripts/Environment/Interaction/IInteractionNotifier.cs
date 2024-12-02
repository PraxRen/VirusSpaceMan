using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractionNotifier
{
    public event Action BeforeInteract;
    public event Action Interacted;
    public event Action AfterInteract;

    bool CanRun();

    void Run();
}