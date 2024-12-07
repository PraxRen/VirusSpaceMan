using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(ActionScheduler))]
public class Interactor : MonoBehaviour, IAction, IReadOnlyInteractor
{
    private const float FactorForwardLimit = 1f;

    [SerializeField][SerializeInterface(typeof(IInteractionNotifier))] private MonoBehaviour _interactionNotifierMonoBehaviour;
    [SerializeField] private TargetTracker _lookTracker;
    [SerializeField] private LayerMask _layerObjectInteraction;

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
    private Coroutine _jobWaitTimerStopInteract;

    public bool IsActive { get; private set; }
    public IReadOnlyObjectInteraction ObjectInteraction => _currentObjectInteraction;
    public LayerMask LayerObjectInteraction => _layerObjectInteraction;

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
        CancelWaitTimerStopInteract();
        StopInteract();
    }

    public bool CanStartInteract(IObjectInteraction objectInteraction)
    {
        return IsActive == false && _interactionNotifier.CanRun() && SimpleUtils.IsLayerInclud(objectInteraction.Layer, _layerObjectInteraction);
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

        if (_currentObjectInteraction.Config.Mode == ModeInteractive.Timer)
            HandleModeTime();

        _currentObjectInteraction.StartInteract();
        _interactionNotifier.Run();
    }

    private void StopInteract()
    {
        _interactionNotifier.Stop();

        if (_currentObjectInteraction != null)
        {
            _currentObjectInteraction.StopInteract();
            StoppedInteract?.Invoke(_currentObjectInteraction);
            _currentObjectInteraction = null;
        }

        _mover.UnblockRotation();
        _lookTracker.ResetTarget();
        _actionScheduler.ClearAction(this);
        IsActive = false;
        //Debug.Log("StopInteract");
    }

    private void OnBeforeInteract()
    {
        _currentObjectInteraction.InteractBefore();
        //Debug.Log("OnBeforeInteract");
    }

    private void OnInteracted()
    {
        _currentObjectInteraction.Interact();
        Interacted?.Invoke(_currentObjectInteraction);
        //Debug.Log("OnInteracted");
    }

    private void OnAfterInteract()
    {
        switch (_currentObjectInteraction.Config.Mode)
        {
            case ModeInteractive.Default:
                HandleModeDefault();
                break;
            case ModeInteractive.Loop:
            case ModeInteractive.Timer:
                HandleModeLoop();
                break;
        }

        //Debug.Log("OnAfterInteract");
    }

    private void HandleModeDefault()
    {
        _currentObjectInteraction.InteractAfter();
        StopInteract();
    }

    private void HandleModeLoop()
    {
        CancelWaitAnimationLoopTimeout();
        _jobWaitAnimationLoopTimeout = StartCoroutine(WaitAnimationLoopTimeout());
    }

    private void HandleModeTime()
    {
        CancelWaitTimerStopInteract();
        _jobWaitTimerStopInteract = StartCoroutine(WaitTimerStopInteract());
    }

    private IEnumerator WaitAnimationLoopTimeout()
    {
        _interactionNotifier.Stop();
        yield return new WaitForSeconds(_currentObjectInteraction.Config.AnimationLoopTimeout);
        _currentObjectInteraction.InteractAfter();
        _interactionNotifier.Run();
        _jobWaitAnimationLoopTimeout = null;
    }

    private IEnumerator WaitTimerStopInteract()
    {
        yield return new WaitForSeconds(_currentObjectInteraction.Config.TimeModeTimer);
        StopInteract();
        _jobWaitTimerStopInteract = null;
    }

    private void CancelMoveToObjectInteraction() => CancelCoroutine(ref _jobMoveToObjectInteraction);

    private void CancelWaitAnimationLoopTimeout() => CancelCoroutine(ref _jobWaitAnimationLoopTimeout);

    private void CancelWaitTimerStopInteract() => CancelCoroutine(ref _jobWaitTimerStopInteract);

    private void CancelCoroutine(ref Coroutine coroutine)
    {
        if (coroutine == null)
            return;

        StopCoroutine(coroutine);
        coroutine = null;
    }
}