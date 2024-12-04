using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SwitcherAnimationLayer), typeof(SwitcherAnimationRig))]
public class AnimatorFighter : MonoBehaviour, IAttackNotifier
{
    [SerializeField][SerializeInterface(typeof(IFighterReadOnly))] private MonoBehaviour _fighterMonoBehaviour;
    [Range(0, 1)][SerializeField] private float _weightForAnimationEvent;
    [SerializeField] private float _timeChangeAnimationLayer = 0.15f;

    private Animator _animator;
    private SwitcherAnimationLayer _switcherAnimationLayer;
    private SwitcherAnimationRig _switcherAnimationRig;
    private IFighterReadOnly _fighter;

    public event Action StartingAttack;
    public event Action RunningDamage;
    public event Action StoppingAttack;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _switcherAnimationLayer = GetComponent<SwitcherAnimationLayer>();
        _switcherAnimationRig = GetComponent<SwitcherAnimationRig>();
        _fighter = (IFighterReadOnly)_fighterMonoBehaviour;
    }

    private void OnEnable()
    {
        _fighter.ActivatedWeapon += OnActivatedWeapon;
        _fighter.DeactivatedWeapon += OnDeactivatedWeapon;
    }


    private void OnDisable()
    {
        _fighter.ActivatedWeapon -= OnActivatedWeapon;
        _fighter.DeactivatedWeapon -= OnDeactivatedWeapon;
    }

    public bool CanCreateAttack()
    {
        return _switcherAnimationLayer.IsNotWork && _switcherAnimationRig.IsNotWork;
    }

    public void CreateAttack()
    {
        _animator.SetBool(DataCharacterAnimator.Params.IsAttack, true);
        _animator.SetFloat(DataCharacterAnimator.Params.IndexAttack, _fighter.Weapon.CurrentAttack.AnimationIndex);
    }

    public void CancelAttack()
    {
        _animator.SetBool(DataCharacterAnimator.Params.IsAttack, false);
    }

    private void OnActivatedWeapon(IWeaponReadOnly weapon)
    {
        if (weapon.Config is IAnimationLayerProvider animationLayerProvider)
            _switcherAnimationLayer.SetAnimationLayer(animationLayerProvider.TypeAnimationLayer, _timeChangeAnimationLayer);

        if (weapon.Config is IAnimationRigProvider animationRigProvider)
            _switcherAnimationRig.SetAnimationRig(animationRigProvider.TypeAnimationRig, 1f);
    }

    private void OnDeactivatedWeapon(IWeaponReadOnly weapon)
    {
        if (weapon.Config is IAnimationLayerProvider animationLayerProvider)
            _switcherAnimationLayer.ApplyDefaultAnimationLayer(_timeChangeAnimationLayer);

        if (weapon.Config is IAnimationRigProvider animationRigProvider)
            _switcherAnimationRig.ApplyDefaultAnimationRig();
    }

    //AnimationEvent
    private void OnStartingAttackAnimationEvent(AnimationEvent animationEvent)
    {
        if (_switcherAnimationLayer.GetIndexCurrentMoverAnimationLayer() != animationEvent.intParameter)
            return;

        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;

        StartingAttack?.Invoke();
    }

    //AnimationEvent
    private void OnRunningDamageAnimationEvent(AnimationEvent animationEvent)
    {
        if (_switcherAnimationLayer.GetIndexCurrentMoverAnimationLayer() != animationEvent.intParameter)
            return;

        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;

        RunningDamage?.Invoke();
    }

    //AnimationEvent
    private void OnStoppingAttackAnimationEvent(AnimationEvent animationEvent)
    {
        if (_switcherAnimationLayer.GetIndexCurrentMoverAnimationLayer() != animationEvent.intParameter)
            return;

        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;

        StoppingAttack?.Invoke();
    }
}