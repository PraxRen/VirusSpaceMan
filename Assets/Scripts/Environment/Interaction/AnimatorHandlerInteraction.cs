using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SwitcherAnimationLayer), typeof(SwitcherAnimationRig))]
public class AnimatorHandlerInteraction : MonoBehaviour, IInteractionNotifier
{
    private Animator _animator;
    private SwitcherAnimationLayer _switcherAnimationLayer;
    private SwitcherAnimationRig _switcherAnimationRig;

    public event Action BeforeInteract;
    public event Action Interacted;
    public event Action AfterInteract;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _switcherAnimationLayer = GetComponent<SwitcherAnimationLayer>();
        _switcherAnimationRig = GetComponent<SwitcherAnimationRig>();
    }

    public void Cancel()
    {

    }

    public bool CanRun()
    {
        return _switcherAnimationLayer.IsNotWork && _switcherAnimationRig.IsNotWork;
    }

    public void Run()
    {

    }

    //AnimationEvent
    private void OnBeforeInteract(AnimationEvent animationEvent)
    {
        BeforeInteract?.Invoke();
    }

    //AnimationEvent
    private void OnInteract(AnimationEvent animationEvent)
    {
        Interacted?.Invoke();
    }

    //AnimationEvent
    private void OnAfterInteract(AnimationEvent animationEvent)
    {
        AfterInteract?.Invoke();
    }
}
