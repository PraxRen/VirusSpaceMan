using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(ActionScheduler))]
public class HandlerInteraction : MonoBehaviour, IAction, IReadOnlyHandlerInteraction
{
    private const float FactorForwardLimit = 1f;

    [SerializeField][SerializeInterface(typeof(IInteractionNotifier))] private MonoBehaviour _interactionNotifierMonoBehaviour;
    [SerializeField] private TargetTracker _lookTracker;

    public event Action<IReadOnlyObjectInteraction> StartedInteract;
    public event Action<IReadOnlyObjectInteraction> Interacted;
    public event Action<IReadOnlyObjectInteraction> StoppedInteract;

    private Transform _transform;
    private Mover _mover;
    private ActionScheduler _actionScheduler;
    private IInteractionNotifier _interactionNotifier;
    private Coroutine _jobMoveToObjectInteraction;
    private IObjectInteraction _currentObjectInteraction;
    private Coroutine _jobWaitAnimationLoopTimeout;

    public bool IsActive { get; private set; }
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
        Cancel();
    }

    public void Cancel()
    {
        CancelMoveToObjectInteraction();
        CancelWaitAnimationLoopTimeout();
        StopInteract();
    }

    public bool CanStartInteract(IObjectInteraction objectInteraction)
    {
        return _interactionNotifier.CanRun();
    }

    public void StartInteract(IObjectInteraction objectInteraction)
    {
        IsActive = true;
        _actionScheduler.StartAction(this);
        _actionScheduler.SetBlock(this);
        _currentObjectInteraction = objectInteraction;
        _lookTracker.SetTarget(_currentObjectInteraction.LookAtPoint, Vector3.zero);
        CancelMoveToObjectInteraction();
        _jobMoveToObjectInteraction = StartCoroutine(MoveToObjectInteraction());
        StartedInteract?.Invoke(_currentObjectInteraction);
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


        while (startPoint.CanReach(_transform) == false || Mathf.Approximately(factorForward, FactorForwardLimit) == false)
        {
            factorForward = Vector3.Dot(startPointForward, _transform.forward);
            _mover.LookAtDirection(new Vector2(startPointForward.x, startPointForward.z));
            Vector3 direction = (startPoint.Position - _transform.position).normalized;
            _mover.SimpleMove(new Vector2(direction.x, direction.z));

            yield return null;
        }

        _mover.Cancel();
        _mover.BlockRotation();
        _transform.position = startPoint.Position;
        _transform.forward = startPointForward;
        _jobMoveToObjectInteraction = null;
        _currentObjectInteraction.StartInteract();
        _interactionNotifier.Run();
    }

    private void StopInteract()
    {
        _interactionNotifier.Stop();
        _currentObjectInteraction.StopInteract();
        _mover.UnblockRotation();
        _lookTracker.ResetTarget();
        _currentObjectInteraction = null;
        IsActive = false;
        _actionScheduler.ClearAction(this);
        StoppedInteract?.Invoke(_currentObjectInteraction);
        Debug.Log("StopInteract");
    }

    private void OnBeforeInteract()
    {
        _currentObjectInteraction.InteractBefore();
        Debug.Log("OnBeforeInteract");
    }

    private void OnInteracted()
    {
        _currentObjectInteraction.Interact();
        Interacted?.Invoke(_currentObjectInteraction);
        Debug.Log("OnInteracted");
    }

    private void OnAfterInteract()
    {
        Debug.Log("OnAfterInteract");

        if (_currentObjectInteraction.Config.IsLoop)
        {
            CancelWaitAnimationLoopTimeout();
            _jobWaitAnimationLoopTimeout = StartCoroutine(WaitAnimationLoopTimeout());

            return;
        }

        _currentObjectInteraction.InteractAfter();
        StopInteract();
    }

    private void CancelWaitAnimationLoopTimeout()
    {
        if (_jobWaitAnimationLoopTimeout == null)
            return;

        StopCoroutine(_jobWaitAnimationLoopTimeout);
        _jobWaitAnimationLoopTimeout = null;
    }

    private IEnumerator WaitAnimationLoopTimeout()
    {
        _interactionNotifier.Stop();
        yield return new WaitForSeconds(_currentObjectInteraction.Config.AnimationLoopTimeout);
        _currentObjectInteraction.InteractAfter();
        _interactionNotifier.Run();
        _jobWaitAnimationLoopTimeout = null;
    }
}