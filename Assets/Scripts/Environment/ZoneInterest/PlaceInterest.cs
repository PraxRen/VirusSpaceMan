using System;
using System.Collections;
using UnityEngine;

public class PlaceInterest : MonoBehaviour, IReadOnlyPlaceInterest
{
    [SerializeField][SerializeInterface(typeof(IObjectInteraction))] private MonoBehaviour _objectInteractionMonoBehaviour;
    [SerializeField] private float _radius;

    private Transform _transform;
    private ZoneInterest _zoneInterest;
    private WaitForSeconds _waitUpdateCollision;
    private Coroutine _jobUpdateCollision;
    private IObjectInteraction _objectInteraction;

    public event Action EnteredHandlerInteraction;

    public bool IsEmpty {  get; private set; }
    public bool HasHandlerInteractionInside { get; private set; }
    public IReadOnlyInteractor HandlerInteraction { get; private set; }
    public Vector3 Position => _transform.position;
    public Quaternion Rotation => _transform.rotation;
    public string Name => $"\"{_transform.parent.name}/{name}\"";

    private void Awake()
    {
        _transform = transform;
        IsEmpty = true;
        _objectInteraction = (IObjectInteraction)_objectInteractionMonoBehaviour;
    }

    private void OnEnable()
    {
        if (IsEmpty == false) 
        {
            _jobUpdateCollision = StartCoroutine(UpdateCollision());
        }
    }

    private void OnDisable()
    {
        if (_jobUpdateCollision != null)
        {
            StopCoroutine(_jobUpdateCollision);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = HasHandlerInteractionInside ? Color.green : Color.white;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public void Initialize(ZoneInterest zoneInterest, WaitForSeconds waitUpdateCollision)
    {
        _zoneInterest = zoneInterest;
        _waitUpdateCollision = waitUpdateCollision;
    }

    public void SetHandlerInteraction(IReadOnlyInteractor handlerInteraction)
    {
        if (IsEmpty == false)
            throw new InvalidOperationException("Place not is empty!");

        HandlerInteraction = handlerInteraction;
        IsEmpty = false;
        _jobUpdateCollision = StartCoroutine(UpdateCollision());
        Debug.Log($"SetHandlerInteraction {((Interactor)handlerInteraction).transform.parent.name}");
    }

    public bool TryGetObjectInteraction(IReadOnlyInteractor handlerInteraction, out IObjectInteraction objectInteraction)
    {
        objectInteraction = null;

        if (IsEmpty)
            return false;

        if (HasHandlerInteractionInside == false)
            return false;

        if (handlerInteraction != HandlerInteraction)
            return false;

        if (SimpleUtils.IsLayerInclud(_objectInteraction.Layer, handlerInteraction.LayerObjectInteraction) == false)
            return false;

        objectInteraction = _objectInteraction;
        return true;
    }

    public void Clear()
    {
        HandlerInteraction = null;
        IsEmpty = true;
        Debug.Log("Clear");
    }

    public bool CanReach(Transform transform)
    {
        if (HasHandlerInteractionInside == false)
            return false;

        if (IsHandlerInteraction(transform) == false)
            return false;

        return true;
    }

    private IEnumerator UpdateCollision()
    {
        while (IsEmpty == false)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, _radius, _zoneInterest.LayerMaskHandlerInteraction, QueryTriggerInteraction.Ignore);
            UpdateCollide(colliders);

            yield return _waitUpdateCollision;
        }

        _jobUpdateCollision = null;
    }


    private void UpdateCollide(Collider[] colliders)
    {
        bool isEnterTriggerHandlerInteraction = false;

        foreach (Collider collider in colliders)
        {
            if (IsHandlerInteraction(collider.transform))
            {
                isEnterTriggerHandlerInteraction = true;
                break;
            }
        }

        if (HasHandlerInteractionInside == false && isEnterTriggerHandlerInteraction)
        {
            EnteredHandlerInteraction?.Invoke();
        }
        else if (HasHandlerInteractionInside && isEnterTriggerHandlerInteraction == false)
        {
            Clear();
        }

        HasHandlerInteractionInside = isEnterTriggerHandlerInteraction;
    }

    private bool IsHandlerInteraction(Transform transform)
    {
        if (transform.TryGetComponent(out IReadOnlyInteractor handlerInteraction) == false)
            return false;

        if (handlerInteraction != HandlerInteraction)
            return false;

        return true;
    }
}