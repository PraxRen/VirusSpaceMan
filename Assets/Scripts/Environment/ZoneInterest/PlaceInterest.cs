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

    public event Action EnteredInteractor;

    public bool IsEmpty {  get; private set; }
    public bool HasInteractorInside { get; private set; }
    public IReadOnlyInteractor Interactor { get; private set; }
    public Vector3 Position => _transform.position;
    public Vector3 Center => _transform.position;
    public Quaternion Rotation => _transform.rotation;
    public Axis AxisUp => Axis.Y;
    public Axis AxisForward => Axis.Z;
    public Axis AxisRight => Axis.X;

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
        Gizmos.color = HasInteractorInside ? Color.green : Color.white;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public void HandleSelection() { }

    public void HandleDeselection() { }

    public void Initialize(ZoneInterest zoneInterest, WaitForSeconds waitUpdateCollision)
    {
        _zoneInterest = zoneInterest;
        _waitUpdateCollision = waitUpdateCollision;
    }

    public void Reserve(IReadOnlyInteractor interactor)
    {
        if (IsEmpty == false)
            throw new InvalidOperationException("Place not is empty!");

        Interactor = interactor;
        IsEmpty = false;
        Interactor.Canceled += OnStoppedInteract;
        _jobUpdateCollision = StartCoroutine(UpdateCollision());
    }

    public bool TryGetObjectInteraction(IReadOnlyInteractor handlerInteraction, out IObjectInteraction objectInteraction)
    {
        objectInteraction = null;

        if (IsEmpty)
            return false;

        if (HasInteractorInside == false)
            return false;

        if (handlerInteraction != Interactor)
            return false;

        if (SimpleUtils.IsLayerInclud(_objectInteraction.Layer, handlerInteraction.LayerObjectInteraction) == false)
            return false;

        objectInteraction = _objectInteraction;
        return true;
    }

    public void Clear()
    {
        Interactor.Canceled -= OnStoppedInteract;
        Interactor = null;
        IsEmpty = true;
        HasInteractorInside = false;
    }

    public bool CanReach(Transform transform)
    {
        if (HasInteractorInside == false)
            return false;

        if (IsHandlerInteraction(transform) == false)
            return false;

        return true;
    }

    private IEnumerator UpdateCollision()
    {
        while (IsEmpty == false)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, _radius, _zoneInterest.LayerMaskInteractor, QueryTriggerInteraction.Ignore);
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

        if (HasInteractorInside == false && isEnterTriggerHandlerInteraction)
        {
            EnteredInteractor?.Invoke();
        }
        else if (HasInteractorInside && isEnterTriggerHandlerInteraction == false)
        {
            Clear();
        }

        HasInteractorInside = isEnterTriggerHandlerInteraction;
    }

    private bool IsHandlerInteraction(Transform transform)
    {
        if (transform.TryGetComponent(out IReadOnlyInteractor handlerInteraction) == false)
            return false;

        if (handlerInteraction != Interactor)
            return false;

        return true;
    }

    private void OnStoppedInteract(IReadOnlyInteractor interactor) => Clear();
}