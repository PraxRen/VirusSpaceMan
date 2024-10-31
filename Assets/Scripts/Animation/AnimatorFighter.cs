using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SwitcherAnimationLayer))]
public class AnimatorFighter : MonoBehaviour, IAttackNotifier
{
    [SerializeField][SerializeInterface(typeof(IFighterReadOnly))] private MonoBehaviour _fighterMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IStorageFighter))] private MonoBehaviour _storageFighterMonoBehaviour;
    [Range(0, 1)][SerializeField] private float _weightForAnimationEvent;
    [SerializeField] private float _timeChangeAnimationLayer = 0.15f;

    private Animator _animator;
    private SwitcherAnimationLayer _switcherAnimationLayer;
    private IFighterReadOnly _fighter;
    private IStorageFighter _storageFighter;

    public event Action StartingAttack;
    public event Action RunningDamage;
    public event Action StoppingAttack;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _switcherAnimationLayer = GetComponent<SwitcherAnimationLayer>();
        _fighter = (IFighterReadOnly)_fighterMonoBehaviour;
        _storageFighter = (IStorageFighter)_storageFighterMonoBehaviour;
    }

    private void OnEnable()
    {
        _storageFighter.ChangedWeapon += OnChangedWeapon;
    }

    private void OnDisable()
    {
        _storageFighter.ChangedWeapon -= OnChangedWeapon;
    }

    public void CreateAttack()
    {

    }

    private void OnChangedWeapon(Weapon weapon)
    {
        _switcherAnimationLayer.SetAnimationLayer(weapon.Config.TypeAnimationLayer, _timeChangeAnimationLayer);
    }

    //AnimationEvent
    private void OnStartingAttackAnimationEvent(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;
    }

    //AnimationEvent
    private void OnRunningDamageAnimationEvent(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;
    }

    //AnimationEvent
    private void OnStoppingAttackAnimationEvent(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;
    }
}