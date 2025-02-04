using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SwitcherAnimationLayer), typeof(SwitcherAnimationRig))]
public class AnimatorInteractor : MonoBehaviour, IInteractionNotifier
{
    [SerializeField][SerializeInterface(typeof(IReadOnlyInteractor))] private MonoBehaviour _handlerInteractionMonoBehaviour;
    [Range(0, 1)][SerializeField] private float _weightForAnimationEvent;
    [SerializeField] private float _timeChangeAnimationLayer = 0.15f;

    private Animator _animator;
    private SwitcherAnimationLayer _switcherAnimationLayer;
    private SwitcherAnimationRig _switcherAnimationRig;
    private IReadOnlyInteractor _handlerInteraction;
    private SettingAnimationLayer _beforeRunTypeAnimationLayer;
    private TypeAnimationRig _beforeRunTypeAnimationRig;
    private bool _isRunning;
    private GameObject _gameObject;

    public event Action BeforeInteract;
    public event Action Interacted;
    public event Action AfterInteract;

    private void Awake()
    {
        _gameObject = gameObject;
        _animator = GetComponent<Animator>();
        _switcherAnimationLayer = GetComponent<SwitcherAnimationLayer>();
        _switcherAnimationRig = GetComponent<SwitcherAnimationRig>();
        _handlerInteraction = (IReadOnlyInteractor)_handlerInteractionMonoBehaviour;
    }

    public bool CanRun()
    {
        return _switcherAnimationLayer.IsNotWork;
    }

    public void Run()
    {
        if (_handlerInteraction.ObjectInteraction.Config is IAnimationLayerProvider animationLayerProvider)
        {
            _beforeRunTypeAnimationLayer = _switcherAnimationLayer.CurrentSetting;
            _switcherAnimationLayer.SetSetting(animationLayerProvider.SettingAnimationLayer, _timeChangeAnimationLayer);
        }

        if (_handlerInteraction.ObjectInteraction.Config is IAnimationRigProvider animationRigProvider)
        {
            _beforeRunTypeAnimationRig = _switcherAnimationRig.CurrentTypeAnimationRig;
            _switcherAnimationRig.SetAnimationRig(animationRigProvider.TypeAnimationRig);
        }

        _isRunning = true;
        _animator.SetFloat(DataCharacterAnimator.Params.IndexInteractive, _handlerInteraction.ObjectInteraction.IdIteration);
        _animator.SetBool(DataCharacterAnimator.Params.IsInteractive, true);
    }

    public void Stop()
    {
        _isRunning = false;

        if (enabled == false)
            return;

        if (_gameObject.activeInHierarchy == false)
            return;

        if (_handlerInteraction.ObjectInteraction != null)
        {
            if (_beforeRunTypeAnimationLayer != null && _handlerInteraction.ObjectInteraction.Config is IAnimationLayerProvider animationLayerProvider)
                _switcherAnimationLayer.SetSetting(_beforeRunTypeAnimationLayer, _timeChangeAnimationLayer);

            if (_beforeRunTypeAnimationRig != TypeAnimationRig.None && _handlerInteraction.ObjectInteraction.Config is IAnimationRigProvider animationRigProvider)
                _switcherAnimationRig.SetAnimationRig(_beforeRunTypeAnimationRig);
        }

        _animator.SetBool(DataCharacterAnimator.Params.IsInteractive, false);
    }

    //AnimationEvent
    private void OnInteractBefore(AnimationEvent animationEvent)
    {
        if (_switcherAnimationLayer.GetIndexCurrentSetting() != animationEvent.intParameter)
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
        if (_switcherAnimationLayer.GetIndexCurrentSetting() != animationEvent.intParameter)
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
        if (_switcherAnimationLayer.GetIndexCurrentSetting() != animationEvent.intParameter)
            return;

        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;

        if (_isRunning == false)
            return;

        //Debug.Log(animationEvent.animatorClipInfo.clip.name);
        AfterInteract?.Invoke();
    }
}