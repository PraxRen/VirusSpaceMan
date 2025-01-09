using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Trigger : MonoBehaviour, IReadOnlyTrigger
{
    [SerializeField] private float _radius;
    [SerializeField] private float _timeWaitUpdate;
    [SerializeField] private int _maxCountTargets;
    [SerializeField] private LayerMask _layerMask;

    private Transform _transform;
    private WaitForSeconds _waitFindTarget;
    private Coroutine _jobFindTarget;
    private Collider[] _hashColliders; 

    public event Action<Collider> BeforeChangedTarget;
    public event Action<Collider> ChangedTarget;
    public event Action<Collider> RemovedTarget;

    public Collider Target { get; private set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    private void Awake()
    {
        _transform = transform;
        _waitFindTarget = new WaitForSeconds(_timeWaitUpdate);
        _hashColliders = new Collider[_maxCountTargets];
    }

    private void OnEnable()
    {
        CancelFindTarget();
        _jobFindTarget = StartCoroutine(FindTarget());
    }

    private IEnumerator FindTarget()
    {
        while (true)
        {
            yield return _waitFindTarget;
            int countResult = Physics.OverlapSphereNonAlloc(_transform.position, _radius, _hashColliders, _layerMask, QueryTriggerInteraction.Ignore);

            if (countResult == 0)
            {
                if (Target != null)
                    RemovedTarget?.Invoke(Target);

                Target = null;
                continue;
            }

            Collider firstTarget = _hashColliders.OrderBy(target => (target.transform.position - _transform.position).sqrMagnitude).First();

            if (firstTarget == Target)
                continue;

            BeforeChangedTarget?.Invoke(firstTarget);
            Target = firstTarget;
            ChangedTarget?.Invoke(Target);
        }
    }

    private void OnDisable()
    {
        CancelFindTarget();
    }

    private void CancelFindTarget()
    {
        if (_jobFindTarget == null)
            return;

        StopCoroutine(_jobFindTarget);
        _jobFindTarget = null;
    }
}