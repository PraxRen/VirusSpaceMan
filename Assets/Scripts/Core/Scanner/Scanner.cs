using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scanner : MonoBehaviour, IReadOnlyScanner
{
    [SerializeField][SerializeInterface(typeof(IScannerStrategy))] private ScriptableObject _scannerStrategyMonoBehaviour;
    [SerializeField] private float _defaultFrequencyUpdate;
    [SerializeField] private float _radiusBase;
    [SerializeField] private LayerMask _layerMask;

    private List<Collider> _targets = new List<Collider>();
    private int _currentIndexTarget = -1;
    private IScannerStrategy _scannerStrategy;
    private Transform _transform;
    private WaitForSeconds _waitUpdateScan;
    private Coroutine _jobScanTargets;

    public event Action<IReadOnlyCollection<Collider>> ChangedTargets;
    public event Action<Collider> BeforeChangedCurrentTarget;
    public event Action<Collider> ChangedCurrentTarget;
    public event Action<Collider> RemovedCurrentTarget;
    public event Action<float> ChangedRadius;

    public Collider Target { get; private set; }
    public float Radius { get; private set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Mathf.Max(Radius, _radiusBase));
    }

    private void Awake()
    {
        _transform = transform;
        _scannerStrategy = (IScannerStrategy)_scannerStrategyMonoBehaviour;
        _waitUpdateScan = new WaitForSeconds(_defaultFrequencyUpdate);
    }

    private void OnDisable()
    {
        CancelUpdateScan();
    }

    public void StartScan(float radius)
    {
        if (enabled == false)
            return;

        if (gameObject.activeInHierarchy == false)
            return;

        CancelUpdateScan();
        UpdateRadius(radius);
        _jobScanTargets = StartCoroutine(ScanTargets());
    }

    public void ResetRadius()
    {
        UpdateRadius(_radiusBase);
    }

    public void UpdateRadius(float radius)
    {
        Debug.Log($"UpdateRadius: {radius}");
        Radius = Mathf.Max(_radiusBase, radius);
        ChangedRadius?.Invoke(Radius);
    }

    public void NextTarget()
    {
        if (_targets.Count == 0)
            return;

        _currentIndexTarget = _currentIndexTarget == _targets.Count - 1 ? 0 : _currentIndexTarget + 1;
        UpdateTarget();
    }

    public void PreviousTarget()
    {
        if (_targets.Count == 0)
            return;

        _currentIndexTarget = _currentIndexTarget == 0 ? _targets.Count - 1 : _currentIndexTarget - 1;
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        if (_targets.Count == 0 && Target != null)
        {
            RemovedCurrentTarget?.Invoke(Target);
            Target = null;
            return;
        }

        Collider newTarget = _targets[_currentIndexTarget];

        if (Target == newTarget) 
            return;

        BeforeChangedCurrentTarget?.Invoke(newTarget);
        Target = newTarget;
        ChangedCurrentTarget?.Invoke(Target);
    }

    private IEnumerator ScanTargets()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, Radius, _layerMask, QueryTriggerInteraction.Ignore);
            UpdateTargets(colliders);
            yield return _waitUpdateScan;
        }
    }

    private void UpdateTargets(Collider[] hitColliders)
    {
        bool isStartEmptyTargets = _targets.Count == 0;
        bool isChangedTargets = false;

        if (hitColliders.Length == 0 && isStartEmptyTargets == false)
        {
            _targets.Clear();
            isChangedTargets = true;
        }

        foreach (Collider hitCollider in hitColliders) 
        {
            if (_targets.Contains(hitCollider) == false)
            {
                _targets.Add(hitCollider);
                isChangedTargets = true;
            }
        }

        foreach (Collider hitCollider in _targets.ToList())
        {
            if (hitColliders.Contains(hitCollider) == false)
            {
                _targets.Remove(hitCollider);
                isChangedTargets = true;
            }
        }

        if (isChangedTargets == false)
            return;

        _targets = _scannerStrategy.Sort(_targets, _transform).ToList();
        ChangedTargets?.Invoke(_targets);

        if (isStartEmptyTargets && _targets.Count > 0)
            _currentIndexTarget = 0;

        if (_currentIndexTarget >= _targets.Count)
            _currentIndexTarget = _targets.Count - 1;

        UpdateTarget();
    }

    private void CancelUpdateScan()
    {
        if (_jobScanTargets == null)
            return;

        StopCoroutine(_jobScanTargets);
        _jobScanTargets = null;
    }
}