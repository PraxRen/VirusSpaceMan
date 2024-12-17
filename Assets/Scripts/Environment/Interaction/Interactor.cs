using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(ActionScheduler))]
public class Interactor : MonoBehaviour, IAction, IReadOnlyInteractor
{
    private const float FactorForwardLimit = 1f;

    [SerializeField][SerializeInterface(typeof(IInteractionNotifier))] private MonoBehaviour _interactionNotifierMonoBehaviour;
    [SerializeField] private SwitcherGraphics _switcherGraphics;
    [SerializeField] private TargetTracker _lookTracker;
    [SerializeField] private LayerMask _layerObjectInteraction;

    public event Action<IReadOnlyInteractor, IReadOnlyObjectInteraction> StartedInteract;
    public event Action<IReadOnlyInteractor, IReadOnlyObjectInteraction> Interacted;
    public event Action<IReadOnlyInteractor, IReadOnlyObjectInteraction> StoppedInteract;

    private Transform _transform;
    private Mover _mover;
    private ActionScheduler _actionScheduler;
    private IInteractionNotifier _interactionNotifier;
    private Coroutine _jobMoveToObjectInteraction;
    private IObjectInteraction _currentObjectInteraction;
    private List<SettingIterationInteraction> _settingIterations;
    private IEnumerable<TypeGraphics> _activeGraphics;
    private Coroutine _jobWaitAnimationLoopTimeout;
    private Coroutine _jobWaitTimerStopInteract;

    public bool IsActive { get; private set; }
    public IReadOnlyObjectInteraction ObjectInteraction => _currentObjectInteraction;
    public LayerMask LayerObjectInteraction => _layerObjectInteraction;
    public Vector3 Position => _transform.position;
    public Quaternion Rotation => _transform.rotation;

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
        _settingIterations = _currentObjectInteraction.Config.SettingIterations.ToList();
        _lookTracker.SetTarget(_currentObjectInteraction.LookAtPoint, Vector3.zero);
        CancelMoveToObjectInteraction();
        _jobMoveToObjectInteraction = StartCoroutine(MoveToObjectInteraction());
        StartedInteract?.Invoke(this, _currentObjectInteraction);
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

            if (startPoint.CanReach(_transform) == false)
            {
                Vector3 direction = (startPoint.Position - _transform.position).normalized;
                _mover.SimpleMove(new Vector2(direction.x, direction.z));
            }

            yield return null;
        }

        _mover.Cancel();
        _mover.BlockRotation();
        _transform.position = startPoint.Position;
        _transform.forward = startPointForward;
        _jobMoveToObjectInteraction = null;

        if (_currentObjectInteraction.Config.Mode == ModeInteractive.Timer)
            HandleModeTime();

        UpdateGraphics(null, _settingIterations[_currentObjectInteraction.IndexIteration].StartInteractGraphics);
        _currentObjectInteraction.StartInteract();
        _interactionNotifier.Run();
    }

    private void StopInteract()
    {
        _interactionNotifier.Stop();

        if (_currentObjectInteraction != null)
        {
            _currentObjectInteraction.StopInteract();
            UpdateGraphics(_activeGraphics, _settingIterations[_currentObjectInteraction.IndexIteration].StopInteractGraphics);
            StoppedInteract?.Invoke(this, _currentObjectInteraction);
            _currentObjectInteraction = null;
            _settingIterations = null;
        }

        _mover.UnblockRotation();
        _lookTracker.ResetTarget();
        _actionScheduler.ClearAction(this);
        IsActive = false;
        //Debug.Log("StopInteract");
    }

    private void OnBeforeInteract()
    {
        UpdateGraphics(_activeGraphics, _settingIterations[_currentObjectInteraction.IndexIteration].BeforeInteractGraphics);
        _currentObjectInteraction.InteractBefore();
        //Debug.Log("OnBeforeInteract");
    }

    private void OnInteracted()
    {
        UpdateGraphics(_activeGraphics, _settingIterations[_currentObjectInteraction.IndexIteration].InteractedGraphics);
        _currentObjectInteraction.Interact();
        Interacted?.Invoke(this, _currentObjectInteraction);
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
        UpdateGraphics(_activeGraphics, _settingIterations[_currentObjectInteraction.IndexIteration].AfterInteractGraphics);
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
        UpdateGraphics(_activeGraphics, _settingIterations[_currentObjectInteraction.IndexIteration].AfterInteractGraphics);
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

    private void UpdateGraphics(IEnumerable<TypeGraphics> oldGraphics, IEnumerable<TypeGraphics> newGraphics)
    {
        oldGraphics ??= Enumerable.Empty<TypeGraphics>();
        newGraphics ??= Enumerable.Empty<TypeGraphics>();

        IEnumerable<TypeGraphics> removedGraphics = oldGraphics.Except(newGraphics);

        foreach (var graphics in removedGraphics)
            _switcherGraphics.Deactivate(graphics);

        IEnumerable<TypeGraphics> addedGraphics = newGraphics.Except(oldGraphics);

        foreach (var graphics in addedGraphics)
            _switcherGraphics.Activate(graphics);

        _activeGraphics = addedGraphics.Count() == 0 ? newGraphics : addedGraphics;
        //Debug.Log(string.Join(" ", _activeGraphics.Select(x => x.ToString()).ToArray()));
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

    public bool CanReach(Transform transform)
    {
        throw new NotImplementedException();
    }
}