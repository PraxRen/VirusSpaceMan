using System.Linq;
using UnityEngine;

public class AISetterTargetTracker : MonoBehaviour
{
    [SerializeField] private TargetTracker _moveTargetTracker;
    [SerializeField] private TargetTracker _lookTargetTracker;
    [SerializeField][SerializeInterface(typeof(IDamageable))] private MonoBehaviour _mainDamageableMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyListenerSimpleEvent))] private MonoBehaviour _listenerSimpleEventMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyTrigger))] private MonoBehaviour _triggerMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyScanner))] private MonoBehaviour _scannerMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyActivatorRagdoll))] private MonoBehaviour _activatorRagdollMonoBehaviour; 

    private IDamageable _mainDamageable;
    private IReadOnlyListenerSimpleEvent _listenerSimpleEvent;
    private IReadOnlyTrigger _trigger;
    private IReadOnlyScanner _scanner;
    private IReadOnlyActivatorRagdoll _activatorRagdoll;

    private void Awake()
    {
        _mainDamageable = (IDamageable)_mainDamageableMonoBehaviour;
        _listenerSimpleEvent = (IReadOnlyListenerSimpleEvent)_listenerSimpleEventMonoBehaviour;
        _trigger = (IReadOnlyTrigger)_triggerMonoBehaviour;
        _scanner = (IReadOnlyScanner)_scannerMonoBehaviour;
        _activatorRagdoll = (IReadOnlyActivatorRagdoll)_activatorRagdollMonoBehaviour;
    }

    private void OnEnable()
    {
        _mainDamageable.BeforeTakeDamage += OnBeforeTakeDamage;
        _listenerSimpleEvent.BeforeNotified += OnBeforeNotified;
        _trigger.BeforeChangedTarget += OnBeforeChangedTarget;
        _trigger.RemovedTarget += ClearTargets;
        _scanner.BeforeChangedCurrentTarget += OnBeforeChangedTarget;
        _scanner.ClearTargets += ClearTargets;
        _activatorRagdoll.BeforeActivated += OnBeforeActivated;
    }


    private void OnDisable()
    {
        _mainDamageable.BeforeTakeDamage -= OnBeforeTakeDamage;
        _listenerSimpleEvent.BeforeNotified -= OnBeforeNotified;
        _trigger.BeforeChangedTarget -= OnBeforeChangedTarget;
        _trigger.RemovedTarget -= ClearTargets;
        _scanner.BeforeChangedCurrentTarget -= OnBeforeChangedTarget;
        _scanner.ClearTargets -= ClearTargets;
        _activatorRagdoll.BeforeActivated -= OnBeforeActivated;
    }

    private void SetTarget(ITarget target)
    {
        _moveTargetTracker.SetTarget(target, Vector3.zero);
        _lookTargetTracker.SetTarget(target, target.Center - target.Position);
    }

    private void OnBeforeTakeDamage(Hit hit, float damage)
    {
        IFighterReadOnly fighter = hit.Weapon.Fighter;
        SetTarget(fighter);
    }

    private void OnBeforeNotified(ISimpleEventCreator creatorSimpleEven, ISimpleEventInitiator simpleEventInitiator, SimpleEvent simpleEvent)
    {
        if (simpleEvent.Type != TypeSimpleEvent.Attack)
            return;

        ITarget target = simpleEventInitiator;

        if (creatorSimpleEven is Fighter fighter)
        {
            if (fighter.TryGetComponent(out Player player))
            {
                target = creatorSimpleEven;
            }
        }

        SetTarget(target);
    }

    private void OnBeforeChangedTarget(Collider collider)
    {
        if (collider.TryGetComponent(out IDamageable damageable) == false)
            return;

        SetTarget(damageable);
    }

    private void OnBeforeActivated(Hit hit)
    {
        IFighterReadOnly fighter = hit.Weapon.Fighter;
        SetTarget(fighter);
    }

    private void ClearTargets()
    {
        _lookTargetTracker.ResetTarget();
    }
}