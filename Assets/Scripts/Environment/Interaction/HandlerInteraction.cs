using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(ActionScheduler))]
public class HandlerInteraction : MonoBehaviour, IAction, IReadOnlyHandlerInteraction
{
    private const float FactorForwardLimit = 0.9f;
    private const float FixDeltaUpdatePosition = 2f;
    private const float FixDeltaUpdateRotation = 10f;

    [SerializeField][SerializeInterface(typeof(IInteractionNotifier))] private MonoBehaviour _interactionNotifierMonoBehaviour;

    private Transform _transform;
    private Mover _mover;
    private ActionScheduler _actionScheduler;
    private IInteractionNotifier _interactionNotifier;
    private Coroutine _jobMoveToObjectInteraction;
    private IObjectInteraction _currentObjectInteraction;

    public IReadOnlyObjectInteraction ObjectInteraction => _currentObjectInteraction;

    private void Awake()
    {
        _transform = transform;
        _mover = GetComponent<Mover>();
        _actionScheduler = GetComponent<ActionScheduler>();
        _interactionNotifier = (IInteractionNotifier)_interactionNotifierMonoBehaviour;
    }

    private void OnEnable()
    {
        _interactionNotifier.BeforeInteract += OnBeforeInteract;
        _interactionNotifier.Interacted += OnInteracted;
        _interactionNotifier.AfterInteract += OnAfterInteract;
    }

    private void OnDisable()
    {
        _interactionNotifier.BeforeInteract -= OnBeforeInteract;
        _interactionNotifier.Interacted -= OnInteracted;
        _interactionNotifier.AfterInteract -= OnAfterInteract;
    }

    public void Cancel()
    {
        CancelMoveToObjectInteraction();
        _interactionNotifier.Stop();
    }

    public bool CanStartInteract(IObjectInteraction objectInteraction)
    {
        return _interactionNotifier.CanRun();
    }

    public void StartInteract(IObjectInteraction objectInteraction)
    {
        _actionScheduler.StartAction(this);
        _actionScheduler.SetBlock(this);
        _currentObjectInteraction = objectInteraction;
        CancelMoveToObjectInteraction();
        _jobMoveToObjectInteraction = StartCoroutine(MoveToObjectInteraction());
    }

    private void CancelMoveToObjectInteraction()
    {
        if (_jobMoveToObjectInteraction == null)
            return;

        StopCoroutine(_jobMoveToObjectInteraction);
        _jobMoveToObjectInteraction = null;
    }

    private IEnumerator MoveToObjectInteraction()
    {
        float factorForward = -1;
        ITarget startPoint = _currentObjectInteraction.StartPoint;
        Vector3 startPointForward = startPoint.Rotation * Vector3.forward;

        while (startPoint.CanReach(_transform) == false && factorForward < FactorForwardLimit) 
        {
            factorForward = Vector3.Dot(startPointForward, _transform.forward);

            if (_mover.CanMove())
            {
                Vector3 directionVector3 = (startPoint.Position - _transform.position).normalized;
                Vector2 direction = new Vector2(directionVector3.x, directionVector3.z);
                _mover.LookAtDirection(direction);
                _mover.Move(direction);
            }
            else
            {
                _transform.forward = Vector3.MoveTowards(_transform.forward, startPointForward, FixDeltaUpdatePosition * Time.fixedDeltaTime);
                _transform.position = Vector3.MoveTowards(_transform.position, startPoint.Position, FixDeltaUpdateRotation * Time.deltaTime);
            }

            yield return null;
        }

        _transform.position = startPoint.Position;
        _transform.forward = startPointForward;
        _jobMoveToObjectInteraction = null;
        _interactionNotifier.Run();
    }

    private void OnBeforeInteract()
    {
        _currentObjectInteraction.InteractBefore();
    }

    private void OnInteracted()
    {
        _currentObjectInteraction.Interact();
    }

    private void OnAfterInteract()
    {
        _currentObjectInteraction.InteractAfter();
        _actionScheduler.ClearAction(this);
    }
}