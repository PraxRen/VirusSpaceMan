using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour, IReadOnlyState, ISerializationCallbackReceiver
{
    [SerializeField][ReadOnly] private string _id;
    [Range(0f,1f)][SerializeField] private float _timeSecondsWaitHandle;
    [SerializeField] private List<Transition> _transitions;

    public event Action<StatusState> ChangedStatus;
    public event Action<IReadOnlyState> GetedNextState;

    public string Id => _id;
    public StatusState Status { get; private set; }
    public IReadOnlyCollection<IReadOnlyTransition> Transitions => _transitions;
    public WaitForSeconds WaitHandle { get; private set; }
    protected Character Character { get; private set; }

    public void Initialize(Character character)
    {
        if (Status != StatusState.None)
            throw new InvalidOperationException($"—осто€ние \"{GetType().Name}\" уже инициализировано!");

        InitializeBeforeAddon(character);
        Character = character;
        WaitHandle = new WaitForSeconds(_timeSecondsWaitHandle);

        foreach (var transition in _transitions)
        {
            transition.Initialize(character, this);
        }

        InitializeAddon();
        UpdateStatus(StatusState.Initialized);
        InitializeAfterAddon();
    }

    public void Enter()
    {
        if (Status != StatusState.Initialized && Status != StatusState.Exited)
            throw new InvalidOperationException($"Ќевозможно изменить статус состо€ни€ \"{GetType().Name}\" на \"{StatusState.Entered}\"!");

        EnterBeforeAddon();
        ActivateTransitions(out Transition transitionNeedTransit);
        EnterAddon();
        UpdateStatus(StatusState.Entered);
        EnterAfterAddon();

        if (transitionNeedTransit != null)
        {
            OnChangedTransitionStatus(transitionNeedTransit, StatusTransition.NeedTransit);
        }
    }

    public void Exit()
    {
        if ((int)Status < (int)StatusState.Entered)
            throw new InvalidOperationException($"Ќевозможно изменить статус состо€ни€ \"{GetType().Name}\" на \"{StatusState.Exited}\"!");

        ExitBeforeAddon();
        DeactivateTransitions();
        Complete();
        ExitAddon();
        UpdateStatus(StatusState.Exited);
        ExitAfterAddon();
    }

    public void Handle()
    {
        if (CanHandle() == false)
            return;

        HandleAddon();
    }

    protected virtual void InitializeBeforeAddon(IReadOnlyCharacter aiCharacter) { }

    protected virtual void InitializeAddon() { }

    protected virtual void InitializeAfterAddon() { }

    protected virtual void EnterBeforeAddon() { }

    protected virtual void EnterAddon() { }

    protected virtual void EnterAfterAddon() { }

    protected virtual void ExitBeforeAddon() { }

    protected virtual void ExitAddon() { }

    protected virtual void ExitAfterAddon() { }

    protected virtual bool CanHandleAddon() => true;

    protected virtual void HandleAddon() { }

    protected void Complete() => UpdateStatus(StatusState.Completed);

    private void ActivateTransitions(out Transition transitionNeedTransit)
    {
        transitionNeedTransit = null;

        foreach (var transition in _transitions)
        {
            transition.ChangedStatus += OnChangedTransitionStatus;
            transition.Activate();

            if (transition.Status == StatusTransition.NeedTransit)
            {
                transitionNeedTransit = transition;
                break;
            }
        }
    }

    private void DeactivateTransitions()
    {
        foreach (var transition in _transitions)
        {
            transition.ChangedStatus -= OnChangedTransitionStatus;
            transition.Deactivate();
        }
    }
    
    private bool CanHandle()
    {
        if (Status != StatusState.Entered)
            return false;

        return CanHandleAddon();
    }

    private void UpdateStatus(StatusState state)
    {
        if (Status == state)
            return;

        Status = state;
        ChangedStatus?.Invoke(Status);
    }

    private void OnChangedTransitionStatus(IReadOnlyTransition transition, StatusTransition statusTransition)
    {
        if (statusTransition != StatusTransition.NeedTransit)
            return;

        GetedNextState?.Invoke(transition.TargetState);
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (string.IsNullOrWhiteSpace(_id))
            _id = Guid.NewGuid().ToString();
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize() { }
}