using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivatorInteractiveDEBUG : MonoBehaviour
{
    [SerializeField] private HandlerInteraction _handlerInteraction;
    [SerializeField] private LayerMask _objectInteractionLayer;
    [SerializeField] private float _radius;
    [SerializeField] private bool _canInteract = true;

    private void Update()
    {
        if (_canInteract == false)
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
                _canInteract = false;
            }
        }
    }
}
