using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _timeWaitUpdate;
    [SerializeField] private LayerMask _layerMask;

    private Transform _transform;
    private WaitForSeconds _waitUpdateScan;
    private Coroutine _jobTargets;

    public Collider Target { get; private set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    private void Awake()
    {
        _transform = transform;
        _waitUpdateScan = new WaitForSeconds(_timeWaitUpdate);
    }
}
