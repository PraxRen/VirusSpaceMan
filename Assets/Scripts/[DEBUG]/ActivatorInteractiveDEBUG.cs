using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivatorInteractiveDEBUG : MonoBehaviour
{
    [SerializeField] private HandlerInteraction _handlerInteraction;
    [SerializeField] private LayerMask _objectInteractionLayer;
    [SerializeField] private float _radius;
    [SerializeField] private KeyCode _debug;

    private void Update()
    {
        if (Input.GetKeyDown(_debug) == false)
            return;

        Collider[] results = Physics.OverlapSphere(transform.position, _radius, _objectInteractionLayer, QueryTriggerInteraction.Ignore);
        Collider hit = results.FirstOrDefault();

        if (hit == null)
            return;

        if (hit.TryGetComponent(out IObjectInteraction objectInteraction))
        {
            if (_handlerInteraction.CanStartInteract(objectInteraction))
            {
                _handlerInteraction.StartInteract(objectInteraction);
            }
        }
    }
}