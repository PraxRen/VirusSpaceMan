using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ListenerSimpleEvent))]
public class ActivatorSimpleEvent : MonoBehaviour
{
    private Transform _transform;
    private ListenerSimpleEvent _listenerSimpleEvent;

    private void Awake()
    {
        _transform = transform;
        _listenerSimpleEvent = GetComponent<ListenerSimpleEvent>();
    }

    public void Run(ISimpleEventCreator creator, ISimpleEventInitiator initiator, SimpleEvent simpleEvent)
    {
        Collider[] colliders = Physics.OverlapSphere(_transform.position, simpleEvent.Radius, simpleEvent.LayerMask, QueryTriggerInteraction.Ignore);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out ListenerSimpleEvent listenerSimpleEvent) == false)
                continue;

            if (_listenerSimpleEvent == listenerSimpleEvent)
                continue;

            listenerSimpleEvent.Notify(creator, initiator, simpleEvent);
        }
    }
}