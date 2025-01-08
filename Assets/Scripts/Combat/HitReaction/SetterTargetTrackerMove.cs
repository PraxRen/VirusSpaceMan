using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetterTargetTrackerMove : MonoBehaviour
{
    [SerializeField] private TargetTracker _moveTargetTracker;
    [SerializeField] private TargetTracker _lookTargetTracker;
    [SerializeField][SerializeInterface(typeof(IDamageable))] private MonoBehaviour _mainDamageableMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyListenerSimpleEvent))] private MonoBehaviour _listenerSimpleEventMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyTrigger))] private MonoBehaviour _triggerMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyScanner))] private MonoBehaviour _scannerMonoBehaviour;

    private IDamageable _mainDamageable;
    private IReadOnlyListenerSimpleEvent _listenerSimpleEvent;
    private IReadOnlyTrigger _trigger;
    private IReadOnlyScanner _scanner;

    private void Awake()
    {
        _mainDamageable = (IDamageable)_mainDamageableMonoBehaviour;
        _listenerSimpleEvent = (IReadOnlyListenerSimpleEvent)_listenerSimpleEventMonoBehaviour;
        _trigger = (IReadOnlyTrigger)_triggerMonoBehaviour;
        _scanner = (IReadOnlyScanner)_scannerMonoBehaviour;
    }

    private void OnEnable()
    {
        _mainDamageable.BeforeTakeDamage += OnBeforeTakeDamage;
        _listenerSimpleEvent.BeforeNotified += OnBeforeNotified;
        _trigger.BeforeChangedTarget += OnBeforeChangedTarget;
        _scanner.BeforeChangedCurrentTarget += OnBeforeChangedCurrentTarget;
    }

    private void OnDisable()
    {
        _mainDamageable.BeforeTakeDamage -= OnBeforeTakeDamage;
        _listenerSimpleEvent.BeforeNotified -= OnBeforeNotified;
        _trigger.BeforeChangedTarget -= OnBeforeChangedTarget;
        _scanner.BeforeChangedCurrentTarget -= OnBeforeChangedCurrentTarget;
    }

    private void SetTargetCollider(Collider collider)
    {
        if (collider.TryGetComponent(out ITarget target) == false)
            return;

        float center = collider.bounds.center.y - target.Position.y;
        float offsetCenter = 0.2f;
        Vector3 offset = (target.Rotation * Vector3.up) * (center + offsetCenter);
        SetTarget(target, Vector3.zero, offset);
    }

    private void SetTargetFighter(IFighterReadOnly fighter)
    {
        Collider colliderFighter = fighter.IgnoreColliders.FirstOrDefault();
        Vector3 offset = fighter.Rotation * Vector3.up;

        if (colliderFighter != null && colliderFighter.TryGetComponent(out IFighterReadOnly fighterInCollider))
        {
            if (fighterInCollider == fighter)
            {
                float center = colliderFighter.bounds.center.y - fighter.Position.y;
                float offsetCenter = 0.2f;
                offset *= (center + offsetCenter);
            }
        }

        SetTarget(fighter, Vector3.zero, offset);
    }

    private void SetTarget(ITarget target, Vector3 offsetForMove, Vector3 offsetForLook)
    {
        _moveTargetTracker.SetTarget(target, offsetForMove);
        _lookTargetTracker.SetTarget(target, offsetForLook);
    }

    private void OnBeforeTakeDamage(Hit hit, float damage)
    {
        IFighterReadOnly fighter = hit.Weapon.Fighter;
        SetTargetFighter(fighter);
    }

    private void OnBeforeNotified(IReadOnlyCreatorSimpleEvent creatorSimpleEven, ISimpleEventInitiator simpleEventInitiator, SimpleEvent simpleEvent)
    {
        if (simpleEvent.Type != TypeSimpleEvent.Attack)
            return;

        IFighterReadOnly fighter = simpleEvent as IFighterReadOnly;

        if (fighter == null)
            return;

        SetTargetFighter(fighter);
    }

    private void OnBeforeChangedTarget(Collider collider)
    {
        SetTargetCollider(collider);
    }

    private void OnBeforeChangedCurrentTarget(Collider collider)
    {
        SetTargetCollider(collider);
    }
}