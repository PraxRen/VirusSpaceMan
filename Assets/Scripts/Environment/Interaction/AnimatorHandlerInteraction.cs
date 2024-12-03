using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SwitcherAnimationLayer), typeof(SwitcherAnimationRig))]
public class AnimatorHandlerInteraction : MonoBehaviour, IInteractionNotifier
{
    [SerializeField][SerializeInterface(typeof(IReadOnlyHandlerInteraction))] private MonoBehaviour _handlerInteractionMonoBehaviour;
    [Range(0, 1)][SerializeField] private float _weightForAnimationEvent;
    [SerializeField] private float _timeChangeAnimationLayer = 0.15f;

    private Animator _animator;
    private SwitcherAnimationLayer _switcherAnimationLayer;
    private SwitcherAnimationRig _switcherAnimationRig;
    private IReadOnlyHandlerInteraction _handlerInteraction;
    private TypeAnimationLayer _beforeRunTypeAnimationLayer;

    public event Action BeforeInteract;
    public event Action Interacted;
    public event Action AfterInteract;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _switcherAnimationLayer = GetComponent<SwitcherAnimationLayer>();
        _switcherAnimationRig = GetComponent<SwitcherAnimationRig>();
        _handlerInteraction = (IReadOnlyHandlerInteraction)_handlerInteractionMonoBehaviour;
    }

    public bool CanRun()
    {
        return _switcherAnimationLayer.IsNotWork && _switcherAnimationRig.IsNotWork;
    }

    public void Run()
    {
        if (_handlerInteraction.ObjectInteraction is IAnimationLayerProvider animationLayerProvider)
        {
            _beforeRunTypeAnimationLayer = _switcherAnimationLayer.CurrentAnimationLayer;
            _switcherAnimationLayer.SetAnimationLayer(animationLayerProvider.TypeAnimationLayer, _timeChangeAnimationLayer);
        }
    }

    public void Stop()
    {
        if (_handlerInteraction.ObjectInteraction is IAnimationLayerProvider animationLayerProvider)
            _switcherAnimationLayer.SetAnimationLayer(_beforeRunTypeAnimationLayer, _timeChangeAnimationLayer);
    }

    //AnimationEvent
    private void OnBeforeInteract(AnimationEvent animationEvent)
    {
        if (_switcherAnimationLayer.GetIndexCurrentMoverAnimationLayer() != animationEvent.intParameter)
            return;

        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;

        BeforeInteract?.Invoke();
    }

    //AnimationEvent
    private void OnInteract(AnimationEvent animationEvent)
    {
        if (_switcherAnimationLayer.GetIndexCurrentMoverAnimationLayer() != animationEvent.intParameter)
            return;

        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;

        Interacted?.Invoke();
    }

    //AnimationEvent
    private void OnAfterInteract(AnimationEvent animationEvent)
    {
        if (_switcherAnimationLayer.GetIndexCurrentMoverAnimationLayer() != animationEvent.intParameter)
            return;

        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;

        AfterInteract?.Invoke();
    }
}