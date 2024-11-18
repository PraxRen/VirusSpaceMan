using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject, IReadOnlyState, ISerializationCallbackReceiver
{
    [SerializeField][ReadOnly] private string _id;
    [SerializeField] private List<Transition> _transitions;

    public event Action<StatusState> ChangedStatus;
    public event Action<IReadOnlyState> GetedNextState;

    public string Id => _id;
    public StatusState Status { get; private set; }
    public IReadOnlyCharacter Character { get; private set; }
    public IReadOnlyCollection<IReadOnlyTransition> Transitions => _transitions;

    public void Initialize(IReadOnlyCharacter character)
    {
        if (Status != StatusState.None)
            throw new InvalidOperationException($"—осто€ние \"{GetType().Name}\" уже инициализировано!");

        InitializeBeforeAddon(character);
        Character = character;

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

        foreach (var transition in _transitions)
        {
            transition.ChangedStatus -= OnChangedTransitionStatus;
            transition.Deactivate();
        }

        UpdateStatus(StatusState.Completed);
        ExitAddon();
        UpdateStatus(StatusState.Exited);
        ExitAfterAddon();
    }

    public bool CanHandle(float deltaTime)
    {
        if (Status != StatusState.Entered)
            return false;

        return CanUpdateAddon(deltaTime);
    }

    public void Handle(float deltaTime) 
    {
        foreach (Transition transition in _transitions)
        {
            transition.Handle(deltaTime);
        }

        UpdateAddon(deltaTime);
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

    protected virtual bool CanUpdateAddon(float deltaTime) => true;

    protected virtual void UpdateAddon(float deltaTime) { }

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

    private void UpdateStatus(StatusState state)
    {
        if (Status == state)
        {
            return;
        }

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