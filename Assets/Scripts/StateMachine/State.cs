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
            throw new InvalidOperationException($"���������� �������� ������ ��������� \"{GetType().Name}\" �� \"{StatusState.Entered}\"!");

        EnterBeforeAddon();
        ActivateTransitions(out Transition transitionNeedTransit);
        EnterAddon();
        UpdateStatus(StatusState.Entered);

        if (transitionNeedTransit != null)
        {
            OnChangedTransitionStatus(transitionNeedTransit, StatusTransition.NeedTransit);
        }

        EnterAfterAddon();
    }

    public void Exit()
    {
        if ((int)Status < (int)StatusState.Entered)
            throw new InvalidOperationException($"���������� �������� ������ ��������� \"{GetType().Name}\" �� \"{StatusState.Exited}\"!");

        ExitBeforeAddon();
        DeactivateTransitions();
        RemoveTimers();
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
            if (_timers.Contains(timer) == false)
                continue;

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

    protected void Complete() 
    {
        if ((int)Status >= (int)StatusState.Completed)
            return;

        UpdateStatus(StatusState.Completed);
    } 

    protected void AddTimer(Timer timer)
    {
        if (_timers.Contains(timer))
            throw new InvalidOperationException($"\"{GetType().Name}\". This {nameof(Timer)} been added!");

        _timers.Add(timer);
    }

    protected void RemoveTimer(Timer timer)
    {
        if (_timers.Contains(timer) == false)
            return;

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

    private void RemoveTimers()
    {
        foreach(var timer in _timers.ToList())
            RemoveTimer(timer);
    }

    private void UpdateStatus(StatusState state)
    {
        if (Status == state)
            return;

        Status = state;
        //Debug.Log($"UpdateStatus: {Character.Transform.parent.name} | {GetType().Name} = {Status}");
        ChangedStatus?.Invoke(Status);
    }

    private void OnChangedTransitionStatus(IReadOnlyTransition transition, StatusTransition statusTransition)
    {
        if (statusTransition != StatusTransition.NeedTransit)
            return;

        if (Status != StatusState.Entered && Status != StatusState.Completed)
            return;

        GetedNextState?.Invoke(transition.TargetState);
    }
}