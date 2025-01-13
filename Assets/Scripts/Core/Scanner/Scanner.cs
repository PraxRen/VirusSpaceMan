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
    public event Action ClearTargets;
    public event Action<float> ChangedRadius;

    public Collider Target { get; private set; }
    public float Radius { get; private set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Mathf.Max(Radius, _radiusBase));

        if (Target == null)
            return;

        Gizmos.color = Color.black;
        Gizmos.DrawCube(Target.bounds.center, new Vector3(0.28f,2,0.28f));
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
        Radius = Mathf.Max(_radiusBase, radius);
        ChangedRadius?.Invoke(Radius);
    }

    public void NextTarget()
    {
        if (_targets.Count == 0)
            return;

        UpdateSort();
        _currentIndexTarget = _currentIndexTarget == _targets.Count - 1 ? 0 : _currentIndexTarget + 1;
        UpdateTarget();
    }

    public void PreviousTarget()
    {
        if (_targets.Count == 0)
            return;

        UpdateSort();
        _currentIndexTarget = _currentIndexTarget == 0 ? _targets.Count - 1 : _currentIndexTarget - 1;
        UpdateTarget();
    }



    private IEnumerator ScanTargets()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, Radius, _layerMask, QueryTriggerInteraction.Ignore);
            HandleTargets(colliders);
            yield return _waitUpdateScan;
        }
    }

    private void HandleTargets(Collider[] newTargets)
    {
        bool isEmptyNewTargets = newTargets.Length == 0;
        bool isEmptyStartOldTargets = _targets.Count == 0;
        bool hasCurrentTarget = Target != null;

        bool isChangedTargets = false;

        if (isEmptyNewTargets && isEmptyStartOldTargets == false)
        {
            ClearAllTargets();
            return;
        }

        foreach (Collider hitCollider in newTargets) 
        {
            if (_targets.Contains(hitCollider) == false)
            {
                _targets.Add(hitCollider);
                isChangedTargets = true;
            }
        }

        foreach (Collider oldTarget in _targets.ToList())
        {
            if (newTargets.Contains(oldTarget) == false)
            {
                _targets.Remove(oldTarget);
                isChangedTargets = true;

                if (oldTarget == Target)
                    hasCurrentTarget = false;
            }
        }

        if (isChangedTargets == false)
            return;

        _targets = _scannerStrategy.Sort(_targets, _transform).ToList();

        if (hasCurrentTarget)
        {
            _currentIndexTarget = _targets.FindIndex(target => target == Target);
        }
        else
        {
            _currentIndexTarget = 0;
            UpdateTarget();
        }

        ChangedTargets?.Invoke(_targets);
        ////------------------------------------------------------------








        //if (isEmptyStartOldTargets && isEmptyCurrentTarget == false)
        //{
        //    DeselectedTarget?.Invoke(Target);
        //    Target = null;
        //    return;
        //}

        //if (isEmptyCurrentTarget)
        //{
        //    _targets = _scannerStrategy.Sort(_targets, _transform).ToList();

        //    if (isEmptyStartOldTargets && _targets.Count > 0)
        //        _currentIndexTarget = 0;

        //    if (_currentIndexTarget >= _targets.Count)
        //        _currentIndexTarget = _targets.Count - 1;

        //    UpdateTarget();
        //}
        //else
        //{
        //    _currentIndexTarget = _targets.FindIndex(target => target == Target);
        //}
    }

    private void ClearAllTargets()
    {
        _targets.Clear();
        Target = null;
        _currentIndexTarget = -1;
        ClearTargets?.Invoke();
    }

    private void UpdateTarget()
    {
        Collider newTarget = _targets[_currentIndexTarget];
        BeforeChangedCurrentTarget?.Invoke(newTarget);
        Target = newTarget;
        ChangedCurrentTarget?.Invoke(Target);
    }

    private void UpdateSort()
    {
        Collider currentTarget = _targets[_currentIndexTarget];
        _targets = _scannerStrategy.Sort(_targets, _transform).ToList();
        _currentIndexTarget = _targets.FindIndex(target => target == currentTarget);
    }

    private void CancelUpdateScan()
    {
        if (_jobScanTargets == null)
            return;

        StopCoroutine(_jobScanTargets);
        _jobScanTargets = null;
    }
}