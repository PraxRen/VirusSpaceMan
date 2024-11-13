using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SwitcherRagdoll))]
public class ActivatorRagdoll : MonoBehaviour, IHitReaction
{
    private const float ForceYOffset = 0.35f;

    [Min(0f)][SerializeField] private float _timeResetIgnoreColliders;
    [Min(0f)][SerializeField] private float _timeDeactivate;

    private Transform _transform;
    private SwitcherRagdoll _switcherRagdoll;
    private Coroutine _jobRunTimerForDeactivate;

    private void Awake()
    {
        _transform = transform;
        _switcherRagdoll = GetComponent<SwitcherRagdoll>();
    }

    public bool CanHandleHit(IWeaponReadOnly weapon, Vector3 hitPoint, float damage)
    {
        return enabled;
    }

    public void HandleHit(IWeaponReadOnly weapon, Vector3 hitPoint, float damage)
    {
        Vector3 force = CalculateForce(weapon, hitPoint);
        List<Collider> ignoreColliders = new List<Collider>();
        ignoreColliders.AddRange(weapon.Colliders);
        ignoreColliders.AddRange(weapon.Fighter.IgnoreColliders);
        _switcherRagdoll.SetIgnoreColliders(ignoreColliders, true, _timeResetIgnoreColliders);
        _switcherRagdoll.Activete();
        
        if (_jobRunTimerForDeactivate != null)
        {
            StopCoroutine(_jobRunTimerForDeactivate);
            _jobRunTimerForDeactivate = null;
        }

        _jobRunTimerForDeactivate = StartCoroutine(RunTimerForDeactivateRagdoll(_timeDeactivate));

        _switcherRagdoll.ApplyHit(force, hitPoint);
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
}