using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(IFighterReadOnly))]
public class ScannerDamageable : MonoBehaviour
{
    [SerializeField] private float _defaultFrequencyUpdate;
    [SerializeField] private float _radiusBase;

    private Transform _transform;
    private WaitForSeconds _waitUpdateScan;
    private Coroutine _jobUpdateScan;
    private IFighterReadOnly _fighter;
    private float _radius;
    private float _radiusSqr;

    public event Action<IDamageable> ChangedTarget;
    public event Action RemovedTarget;

    public IDamageable Target { get; private set; }
    
    private void Awake()
    {
        _transform = transform;
        _waitUpdateScan = new WaitForSeconds(_defaultFrequencyUpdate);
        _fighter = GetComponent<IFighterReadOnly>();
        UpdateRadius(_radiusBase);
    }

    private void OnEnable()
    {
        _fighter.ChangedWeapon += OnChangedWeapon;
        _fighter.RemovedWeapon += OnRemovedWeapon;
        _jobUpdateScan = StartCoroutine(UpdateScan());
    }

    private void OnDisable()
    {
        _fighter.ChangedWeapon -= OnChangedWeapon;
        _fighter.RemovedWeapon -= OnRemovedWeapon;
        CancelUpdateScan();
    }

    private IEnumerator UpdateScan()
    {
        while (true)
        {
            HandleTarget();
            Collider[] colliders = Physics.OverlapSphere(_transform.position, _radius, _fighter.LayerMaskDamageable, QueryTriggerInteraction.Ignore);
            HandleResultScan(colliders);
            yield return _waitUpdateScan;
        }
    }

    private void HandleTarget()
    {
        if (Target == null)
            return;

        if ((Target.Position - _transform.position).sqrMagnitude > _radiusSqr)
        {
            Target = null;
            RemovedTarget?.Invoke();
        }
    }

    private void HandleResultScan(Collider[] colliders)
    {
        if (colliders == null)
            return;
        
        Collider targetCollider = colliders.Where(collider => (collider.transform.position - _transform.position).sqrMagnitude <= _radiusSqr)
                                           .OrderBy(collider => (collider.transform.position - _transform.position).sqrMagnitude)
                                           .FirstOrDefault();
        if (targetCollider == null)
            return;

        if (targetCollider.TryGetComponent(out IDamageable damageable) == false)
            return;

        if (Target == damageable)
            return;

        Target = damageable;
        ChangedTarget?.Invoke(Target);
    }

    private void CancelUpdateScan()
    {
        if (_jobUpdateScan == null)
            return;

        StopCoroutine(_jobUpdateScan);
        _jobUpdateScan = null;
    }

    private void OnChangedWeapon(IWeaponReadOnly weapon)
    {
        UpdateRadius(_radiusBase + weapon.Config.DistanceAttack);
    }

    private void OnRemovedWeapon()
    {
        UpdateRadius(_radiusBase);
    }

    private void UpdateRadius(float radius)
    {
        _radius = radius;
        _radiusSqr = _radius * _radius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Mathf.Max(_radius, _radiusBase));
    }
}