using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ListenerSimpleEvent))]
public class CreatorSimpleEvent : MonoBehaviour, IReadOnlyCreatorSimpleEvent
{
    [SerializeField] private float _radiusCanReachTarget = 0.3f;
    [SerializeField][SerializeInterface(typeof(ISimpleEventInitiator))] private MonoBehaviour[] _initiatorsMonoBehaviour;

    private Transform _transform;
    private ListenerSimpleEvent _listenerSimpleEvent;
    private ISimpleEventInitiator[] _initiators;

    public Vector3 Position => _transform.position;
    public Quaternion Rotation => _transform.rotation;

    private void Awake()
    {
        _transform = transform;
        _listenerSimpleEvent = GetComponent<ListenerSimpleEvent>();
        _initiators = _initiatorsMonoBehaviour.Cast<ISimpleEventInitiator>().ToArray();
    }

    private void OnEnable()
    {
        foreach (ISimpleEventInitiator initiator in _initiators)
            initiator.SimpleEventStarted += Run;
    }


    private void OnDisable()
    {
        foreach (ISimpleEventInitiator initiator in _initiators)
            initiator.SimpleEventStarted -= Run;
    }

    public bool CanReach(Transform transform) => (transform.position - _transform.position).sqrMagnitude < (_radiusCanReachTarget * _radiusCanReachTarget);

    private void Run(ISimpleEventInitiator initiator, SimpleEvent simpleEvent)
    {
        Collider[] colliders = Physics.OverlapSphere(_transform.position, simpleEvent.Radius, simpleEvent.LayerMask, QueryTriggerInteraction.Ignore);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out ListenerSimpleEvent listenerSimpleEvent) == false)
                continue;

            if (_listenerSimpleEvent == listenerSimpleEvent)
                continue;

            listenerSimpleEvent.Notify(this, initiator, simpleEvent);
        }
    }
}