using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _defaultFrequencyUpdate;
    [SerializeField] private float _radiusBase;

    private Transform _transform;
    private WaitForSeconds _waitUpdateScan;
    private Coroutine _jobUpdateScan;
    private float _radius;
    private float _radiusSqr;
    private LayerMask _layerMask;

    public event Action<Collider> ChangedTarget;
    public event Action RemovedTarget;
    public event Action<float> ChangedRadius;

    public Collider Target { get; private set; }
    
    private void Awake()
    {
        _transform = transform;
        _waitUpdateScan = new WaitForSeconds(_defaultFrequencyUpdate);
    }

    private void OnDisable()
    {
        CancelUpdateScan();
    }

    public void StartScan(LayerMask layerMask)
    {
        StartScan(layerMask, _radiusBase);
    }

    public void StartScan(LayerMask layerMask, float radius)
    {
        CancelUpdateScan();
        UpdateRadius(radius);
        _layerMask = layerMask;
        _jobUpdateScan = StartCoroutine(UpdateScan());
    }

    public void ResetRadius()
    {
        UpdateRadius(_radiusBase);
    }

    public void UpdateRadius(float radius)
    {
        _radius = Mathf.Max(_radiusBase, radius);
        _radiusSqr = _radius * _radius;
        ChangedRadius?.Invoke(_radius);
    }

    private IEnumerator UpdateScan()
    {
        while (true)
        {
            HandleTarget();
            Collider[] colliders = Physics.OverlapSphere(_transform.position, _radius, _layerMask, QueryTriggerInteraction.Ignore);
            HandleResultScan(colliders);
            yield return _waitUpdateScan;
        }
    }

    private void HandleTarget()
    {
        if (Target == null)
            return;

        if ((Target.transform.position - _transform.position).sqrMagnitude > _radiusSqr)
        {
            Target = null;
            RemovedTarget?.Invoke();
        }
    }

    private void HandleResultScan(Collider[] colliders)
    {
        if (colliders == null)
            return;
        
        Collider targetCollider = colliders.Where(collider => collider.transform != _transform && (collider.transform.position - _transform.position).sqrMagnitude <= _radiusSqr)
                                           .OrderBy(collider => (collider.transform.position - _transform.position).sqrMagnitude)
                                           .FirstOrDefault();
        if (targetCollider == null)
            return;

        if (Target == targetCollider)
            return;

        Target = targetCollider;
        ChangedTarget?.Invoke(Target);
    }

    private void CancelUpdateScan()
    {
        if (_jobUpdateScan == null)
            return;

        StopCoroutine(_jobUpdateScan);
        _jobUpdateScan = null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Mathf.Max(_radius, _radiusBase));
    }
#endif
}