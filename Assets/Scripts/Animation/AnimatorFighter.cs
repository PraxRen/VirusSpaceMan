using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SwitcherAnimationLayer))]
public class AnimatorFighter : MonoBehaviour, IAttackNotifier
{
    [SerializeField][SerializeInterface(typeof(IFighterReadOnly))] private MonoBehaviour _fighterMonoBehaviour;
    [Range(0, 1)][SerializeField] private float _weightForAnimationEvent;
    [SerializeField] private float _timeChangeAnimationLayer = 0.15f;

    private Animator _animator;
    private SwitcherAnimationLayer _switcherAnimationLayer;
    private IFighterReadOnly _fighter;

    public event Action StartingAttack;
    public event Action RunningDamage;
    public event Action StoppingAttack;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _switcherAnimationLayer = GetComponent<SwitcherAnimationLayer>();
        _fighter = (IFighterReadOnly)_fighterMonoBehaviour;
    }

    private void OnEnable()
    {
        _fighter.ChangedWeapon += OnChangedWeapon;
        _fighter.RemovedWeapon += OnRemovedWeapon;
    }


    private void OnDisable()
    {
        _fighter.ChangedWeapon -= OnChangedWeapon;
        _fighter.RemovedWeapon -= OnRemovedWeapon;
    }

    public void CreateAttack()
    {

    }

    private void OnChangedWeapon(IWeaponReadOnly weapon)
    {
        if (weapon.Config is not IAnimationLayerProvider animationLayerProvider)
            return;

       _switcherAnimationLayer.SetAnimationLayer(animationLayerProvider.TypeAnimationLayer, _timeChangeAnimationLayer);
    }

    private void OnRemovedWeapon()
    {
        _switcherAnimationLayer.ApplyDefaultAnimationLayer(_timeChangeAnimationLayer);
    }

    //AnimationEvent
    private void OnStartingAttackAnimationEvent(AnimationEvent animationEvent)
    {
        if (_switcherAnimationLayer.GetIndexCurrentMoverAnimationLayer() != animationEvent.intParameter)
            return;

        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;
    }

    //AnimationEvent
    private void OnRunningDamageAnimationEvent(AnimationEvent animationEvent)
    {
        if (_switcherAnimationLayer.GetIndexCurrentMoverAnimationLayer() != animationEvent.intParameter)
            return;

        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;
    }

    //AnimationEvent
    private void OnStoppingAttackAnimationEvent(AnimationEvent animationEvent)
    {
        if (_switcherAnimationLayer.GetIndexCurrentMoverAnimationLayer() != animationEvent.intParameter)
            return;

        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;
    }
}