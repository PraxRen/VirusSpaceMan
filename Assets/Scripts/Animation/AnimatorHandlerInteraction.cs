using System;
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
    private TypeAnimationRig _beforeRunTypeAnimationRig;
    private bool _isRunning;

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
        return _switcherAnimationLayer.IsNotWork;
    }

    public void Run()
    {
        if (_handlerInteraction.ObjectInteraction.Config is IAnimationLayerProvider animationLayerProvider)
        {
            _beforeRunTypeAnimationLayer = _switcherAnimationLayer.CurrentAnimationLayer;
            _switcherAnimationLayer.SetAnimationLayer(animationLayerProvider.TypeAnimationLayer, _timeChangeAnimationLayer);
        }

        if (_handlerInteraction.ObjectInteraction.Config is IAnimationRigProvider animationRigProvider)
        {
            _beforeRunTypeAnimationRig = _switcherAnimationRig.CurrentTypeAnimationRig;
            _switcherAnimationRig.SetAnimationRig(animationRigProvider.TypeAnimationRig);
        }

        _isRunning = true;
        _animator.SetFloat(DataCharacterAnimator.Params.IndexInteractive, _handlerInteraction.ObjectInteraction.AnimationInteractiveIndex);
        _animator.SetBool(DataCharacterAnimator.Params.IsInteractive, true);
    }

    public void Stop()
    {
        _isRunning = false;

        if (_handlerInteraction.ObjectInteraction != null)
        {
            if (_handlerInteraction.ObjectInteraction.Config is IAnimationLayerProvider animationLayerProvider)
                _switcherAnimationLayer.SetAnimationLayer(_beforeRunTypeAnimationLayer, _timeChangeAnimationLayer);

            if (_handlerInteraction.ObjectInteraction.Config is IAnimationRigProvider animationRigProvider)
                _switcherAnimationRig.SetAnimationRig(_beforeRunTypeAnimationRig);
        }

        _animator.SetBool(DataCharacterAnimator.Params.IsInteractive, false);
    }

    //AnimationEvent
    private void OnInteractBefore(AnimationEvent animationEvent)
    {
        if (_switcherAnimationLayer.GetIndexCurrentMoverAnimationLayer() != animationEvent.intParameter)
            return;

        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;

        if (_isRunning == false)
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

        if (_isRunning == false)
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

        if (_isRunning == false)
            return;

        //Debug.Log(animationEvent.animatorClipInfo.clip.name);
        AfterInteract?.Invoke();
    }
}