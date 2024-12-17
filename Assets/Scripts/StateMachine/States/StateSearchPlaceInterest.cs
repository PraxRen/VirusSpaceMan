using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateSearchPlaceInterest : State
{
    private IReadOnlyHandlerEnvironment _handlerEnvironment;
    private IReadOnlyInteractor _interactor;
    private IReadOnlyPlaceInterest _placeInterest;
    private float _timeDelayComplete;
    private bool _isFoundPlace;
    private Timer _timerDelayComplete;

    public StateSearchPlaceInterest(string id, AICharacter character, float timeSecondsWaitHandle, float timeDelayComplete) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _handlerEnvironment) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(IReadOnlyHandlerEnvironment)}\" required for operation \"{GetType().Name}\".");

        if (character.TryGetComponent(out _interactor) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(IReadOnlyInteractor)}\" required for operation \"{GetType().Name}\".");

        _timeDelayComplete = timeDelayComplete;
        _timerDelayComplete = new Timer(_timeDelayComplete);
    }

    public override void Update()
    {
        if (_isFoundPlace)
            return;

        _placeInterest = FindPlaceInterest();

        if (_placeInterest == null)
            return;

        //Debug.Log($"{Character.Transform.parent.name} -- {_placeInterest.Name}");
        _isFoundPlace = true;
        Character.MoveTracker.SetTarget(_placeInterest, Vector3.zero);
        _timerDelayComplete.Completed += OnTimerCompleted;
        AddTimer(_timerDelayComplete);
    }

    protected override void ExitAfterAddon()
    {
        _timerDelayComplete.Completed -= OnTimerCompleted;
        _timerDelayComplete.Reset(_timeDelayComplete);
        _isFoundPlace = false;
        _placeInterest = null;
    }

    private IReadOnlyPlaceInterest FindPlaceInterest()
    {
        _handlerEnvironment.CurrentZone.TryReserveNearestPlaceInterest(_interactor, out IReadOnlyPlaceInterest placeInterest);
        return placeInterest;
    }

    private void OnTimerCompleted(Timer timer)
    {
        timer.Completed -= OnTimerCompleted;
        Complete();
    }
}