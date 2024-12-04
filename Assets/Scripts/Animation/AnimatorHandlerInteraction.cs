using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SwitcherAnimationLayer))]
public class AnimatorHandlerInteraction : MonoBehaviour, IInteractionNotifier
{
    [SerializeField][SerializeInterface(typeof(IReadOnlyHandlerInteraction))] private MonoBehaviour _handlerInteractionMonoBehaviour;
    [Range(0, 1)][SerializeField] private float _weightForAnimationEvent;
    [SerializeField] private float _timeChangeAnimationLayer = 0.15f;

    private Animator _animator;
    private SwitcherAnimationLayer _switcherAnimationLayer;
    private IReadOnlyHandlerInteraction _handlerInteraction;
    private TypeAnimationLayer _beforeRunTypeAnimationLayer;

    public event Action BeforeInteract;
    public event Action Interacted;
    public event Action AfterInteract;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _switcherAnimationLayer = GetComponent<SwitcherAnimationLayer>();
        _handlerInteraction = (IReadOnlyHandlerInteraction)_handlerInteractionMonoBehaviour;
    }

    public bool CanRun()
    {
        return _switcherAnimationLayer.IsNotWork;
    }

    public void Run()
    {
        if (_handlerInteraction.ObjectInteraction is IAnimationLayerProvider animationLayerProvider)
        {
            _beforeRunTypeAnimationLayer = _switcherAnimationLayer.CurrentAnimationLayer;
            _switcherAnimationLayer.SetAnimationLayer(animationLayerProvider.TypeAnimationLayer, _timeChangeAnimationLayer);
        }

        _animator.SetFloat(DataCharacterAnimator.Params.IndexInteractive, _handlerInteraction.ObjectInteraction.IndexAnimation);
        _animator.SetBool(DataCharacterAnimator.Params.IsInteractive, true);
    }

    public void Stop()
    {
        if (_handlerInteraction.ObjectInteraction is IAnimationLayerProvider animationLayerProvider)
            _switcherAnimationLayer.SetAnimationLayer(_beforeRunTypeAnimationLayer, _timeChangeAnimationLayer);

        _animator.SetBool(DataCharacterAnimator.Params.IsInteractive, false);
    }

    //AnimationEvent
    private void OnInteractBefore(AnimationEvent animationEvent)
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
    private void OnInteractAfter(AnimationEvent animationEvent)
    {
        if (_switcherAnimationLayer.GetIndexCurrentMoverAnimationLayer() != animationEvent.intParameter)
            return;

        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;

        AfterInteract?.Invoke();
    }
}