using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateSearchPlaceInterest : State
{
    private IReadOnlyHandlerZoneEnvironment _handlerZoneEnvironment;
    private IReadOnlyInteractor _handlerInteraction;
    private IReadOnlyPlaceInterest _placeInterest;
    private float _timeDelayComplete;
    private bool _isFoundPlace;
    private Timer _timerDelayComplete;

    public StateSearchPlaceInterest(string id, AICharacter character, float timeSecondsWaitHandle, float timeDelayComplete) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _handlerZoneEnvironment) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(IReadOnlyHandlerZoneEnvironment)}\" required for operation \"{GetType().Name}\".");

        if (character.TryGetComponent(out _handlerInteraction) == false)
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
        IReadOnlyList<ZoneInterest> currentZoneInterests = _handlerZoneEnvironment.CurrentZone.Interests;

        if (currentZoneInterests == null)
            return null;

        IReadOnlyPlaceInterest placeInterest = null;
        var activeZoneInterests = currentZoneInterests.Where(zoneInterest => zoneInterest.gameObject.activeSelf)
                                                      .OrderBy(zoneInterest => (zoneInterest.Transform.position - Character.Transform.position).sqrMagnitude);

        foreach (ZoneInterest zoneInterest in activeZoneInterests)
        {
            if (zoneInterest.TryReserveEmptyPlace(_handlerInteraction, out placeInterest))
                break;
        }

        return placeInterest;
    }

    private void OnTimerCompleted(Timer timer)
    {
        timer.Completed -= OnTimerCompleted;
        Complete();
    }
}