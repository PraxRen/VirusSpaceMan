using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateSearchPlaceInterest : State
{
    private IReadOnlyHandlerZoneEnvironment _handlerZoneEnvironment;
    private IReadOnlyHandlerInteraction _handlerInteraction;
    private IReadOnlyPlaceInterest _placeInterest;
    private float _timeDelayComplete;
    private bool _isFoundPlace;

    public StateSearchPlaceInterest(string id, AICharacter character, float timeSecondsWaitHandle, float timeDelayComplete) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _handlerZoneEnvironment) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(IReadOnlyHandlerZoneEnvironment)}\" required for operation \"{GetType().Name}\".");

        if (character.TryGetComponent(out _handlerInteraction) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(IReadOnlyHandlerInteraction)}\" required for operation \"{GetType().Name}\".");

        _timeDelayComplete = timeDelayComplete;
    }

    public override void Update()
    {
        if (_isFoundPlace)
            return;

        _placeInterest = FindPlaceInterest();

        if (_placeInterest == null)
            return;

        _isFoundPlace = true;
        _placeInterest.SetHandlerInteraction(_handlerInteraction);
        Character.MoveTracker.SetTarget(_placeInterest, Vector3.zero);
        Timer timerDelayComplete = new Timer(_timeDelayComplete);
        timerDelayComplete.Completed += OnTimerCompleted;
        AddTimer(timerDelayComplete);
    }

    protected override void ExitAfterAddon()
    {
        _isFoundPlace = false;
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
            if (zoneInterest.TryGetEmptyPlace(_handlerInteraction, out placeInterest))
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