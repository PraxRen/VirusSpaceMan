using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwitcherRagdoll))]
public class ActivatorRagdoll : MonoBehaviour, IHitReaction, IAction
{
    private const float ForceYOffset = 0.35f;

    [SerializeField] private ActionScheduler _actionScheduler;
    [SerializeField][SerializeInterface(typeof(IDamageable))] private MonoBehaviour _mainDamageableMonoBehaviour;
    [Min(0f)][SerializeField] private float _timeResetIgnoreColliders;
    [Min(0f)][SerializeField] private float _timeDeactivate;

    private Transform _transform;
    private SwitcherRagdoll _switcherRagdoll;
    private IDamageable _damageable;
    private Coroutine _jobRunTimerForDeactivate;

    private void Awake()
    {
        _transform = transform;
        _switcherRagdoll = GetComponent<SwitcherRagdoll>();
        _damageable = (IDamageable)_mainDamageableMonoBehaviour;
    }

    private void OnEnable()
    {
        _switcherRagdoll.ExiteAnimationStandUp += OnExiteAnimationStandUp;
    }

    private void OnDisable()
    {
        _switcherRagdoll.ExiteAnimationStandUp -= OnExiteAnimationStandUp;
    }

    public void Cancel()
    {
        CancelJobTimerForDeactivate();
        _switcherRagdoll.Deactivete();
        _actionScheduler.ClearAction(this);
    }

    public bool CanHandleHit(Hit hit, float damage)
    {
        if (enabled == false)
            return false;

        if (_damageable.CanDie(hit, damage) == false)
        {
            if (_actionScheduler.CanStartAction(this) == false)
                return false;

            if (hit.IsRageAttack == false)
                return false;
            
            if (_switcherRagdoll.IsActivated)
                return false;
        }

        return true;
    }

    public void HandleHit(Hit hit, float damage)
    {
        _actionScheduler.StartAction(this);
        _actionScheduler.SetBlock(this);
        Vector3 force = CalculateForce(hit.Weapon, hit.Point);
        List<Collider> ignoreColliders = new List<Collider>();
        ignoreColliders.AddRange(hit.Weapon.Colliders);
        ignoreColliders.AddRange(hit.Weapon.Fighter.IgnoreColliders);
        _switcherRagdoll.SetIgnoreColliders(ignoreColliders, true, _timeResetIgnoreColliders);
        _switcherRagdoll.Activete();

        if (_damageable.CanDie(hit, damage) == false)
        {
            CancelJobTimerForDeactivate();
            _jobRunTimerForDeactivate = StartCoroutine(RunTimerForDeactivateRagdoll(_timeDeactivate));
        }

        _switcherRagdoll.ApplyHit(force, hit.Point);
    }

    private Vector3 CalculateForce(IWeaponReadOnly weapon, Vector3 hitPoint)
    {
        Vector3 forceDirection = CalculateForceDirection(weapon, hitPoint);
        forceDirection.y = ForceYOffset;
        Vector3 force = weapon.Config.Force * forceDirection;
#if UNITY_EDITOR
        Debug.DrawLine(hitPoint, hitPoint + force, Color.yellow, 2f);
#endif
        return force;
    }

    private Vector3 CalculateForceDirection(IWeaponReadOnly weapon, Vector3 hitPoint)
    {
        hitPoint = Vector3.Lerp(hitPoint, weapon.Position, 0.5f);
        Vector3 direction = (hitPoint - new Vector3(_transform.position.x, hitPoint.y, _transform.position.z)).normalized * -1;
        direction.y = 0;
        return direction;
    }

    private IEnumerator RunTimerForDeactivateRagdoll(float timeDeactivate)
    {
        float timer = 0f;

        while (_switcherRagdoll.IsActivated)
        {
            timer += Time.deltaTime;

            if (timer > timeDeactivate)
            {
                _switcherRagdoll.Deactivete();
                break;
            }

            yield return null;
        }

        _jobRunTimerForDeactivate = null;
    }

    private void CancelJobTimerForDeactivate()
    {
        if (_jobRunTimerForDeactivate != null)
        {
            StopCoroutine(_jobRunTimerForDeactivate);
            _jobRunTimerForDeactivate = null;
        }
    }

    private void OnExiteAnimationStandUp()
    {
        _actionScheduler.ClearAction(this);
    }
}