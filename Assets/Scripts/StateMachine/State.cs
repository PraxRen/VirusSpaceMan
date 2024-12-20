using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class State : IReadOnlyState
{
    private List<Transition> _transitions;
    private List<Timer> _timers;

    public event Action<StatusState> ChangedStatus;
    public event Action<IReadOnlyState> GetedNextState;

    public string Id { get; }
    public WaitForSeconds WaitHandle { get; }
    public StatusState Status { get; private set; }
    public IReadOnlyCollection<IReadOnlyTransition> Transitions => _transitions;
    protected AICharacter Character { get; private set; }

    public State(string id, AICharacter character, float timeSecondsWaitHandle)
    {
        Id = id;
        Character = character;
        WaitHandle = new WaitForSeconds(timeSecondsWaitHandle);
        _transitions = new List<Transition>();
        _timers = new List<Timer>();
    }

    public void AddTransition(Transition transition)
    {
        if (_transitions.Contains(transition))
            throw new InvalidOperationException();

        _transitions.Add(transition);
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

    public bool CanUpdate()
    {
        return Status == StatusState.Entered;
    }

    public virtual void Update() { }

    public void Tick(float deltaTime)
    {
        foreach(Transition transition in _transitions)
        {
            transition.Tick(deltaTime);
        }

        foreach(Timer timer in _timers.ToList())
        {
            timer.Tick(deltaTime);
        }

        TickAddon(deltaTime);
    } 

    protected virtual void EnterBeforeAddon() { }

    protected virtual void EnterAddon() { }

    protected virtual void EnterAfterAddon() { }

    protected virtual void ExitBeforeAddon() { }

    protected virtual void ExitAddon() { }

    protected virtual void ExitAfterAddon() { }

    protected virtual void TickAddon(float deltaTime) { }

    protected void Complete() => UpdateStatus(StatusState.Completed);

    protected void AddTimer(Timer timer)
    {
        if (_timers.Contains(timer))
        {
            throw new InvalidOperationException($"This {nameof(Timer)} been added!");
        }

        timer.Completed += OnTimerCompleted;
        _timers.Add(timer);
    }

    private void OnTimerCompleted(Timer timer)
    {
        timer.Completed -= OnTimerCompleted;
        _timers.Remove(timer);
    }

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

    private void UpdateStatus(StatusState state)
    {
        if (Status == state)
            return;

        Status = state;
        Debug.Log($"UpdateStatus: {Character.Transform.parent.name} | {GetType().Name} = {Status}");
        ChangedStatus?.Invoke(Status);
    }

    private void OnChangedTransitionStatus(IReadOnlyTransition transition, StatusTransition statusTransition)
    {
        if (statusTransition != StatusTransition.NeedTransit)
            return;

        if (Status != StatusState.Entered && Status != StatusState.Completed)
            return;

        Debug.Log($"OnChangedTransitionStatus: {Character.Transform.parent.name} | {GetType().Name} = {Status}");
        GetedNextState?.Invoke(transition.TargetState);
    }
}