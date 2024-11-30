using System;
using System.Linq;
using UnityEngine;

public class StateSearchPlaceInterest : State
{
    private IReadOnlyHandlerZoneEnvironment _handlerZoneEnvironment;
    private IReadOnlyPlaceInterest _placeInterest;
    private float _timeDelayComplete;
    private bool _isFoundPlace;

    public StateSearchPlaceInterest(string id, AICharacter character, float timeSecondsWaitHandle, float timeDelayComplete) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _handlerZoneEnvironment) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(HandlerZoneEnvironment)}\" required for operation \"{GetType().Name}\".");

        _timeDelayComplete = timeDelayComplete;
    }

    public override void Update()
    {
        _placeInterest = FindPlaceInterest();

        if (_placeInterest == null)
            return;

        _isFoundPlace = true;
        Character.MoveTracker.SetTarget(_placeInterest, Vector3.zero);
        Timer timerDelayComplete = new Timer(_timeDelayComplete);
        timerDelayComplete.Completed += OnTimer—ompleted;
        AddTimer(timerDelayComplete);
    }

    protected override bool CanUpdateAddon()
    {
        return _isFoundPlace == false;
    }

    protected override void ExitAfterAddon()
    {
        _isFoundPlace = false;
    }

    private IReadOnlyPlaceInterest FindPlaceInterest()
    {
        IReadOnlyPlaceInterest placeInterest = null;

        if (_handlerZoneEnvironment.CurrentZone.Interests == null)
            return null;

        var zoneInterests = _handlerZoneEnvironment.CurrentZone.Interests.Where(zoneInterest => zoneInterest.gameObject.activeSelf)
                                                                         .OrderBy(zoneInterest => (zoneInterest.Transform.position - Character.Transform.position).sqrMagnitude);

        foreach (ZoneInterest zoneInterest in zoneInterests)
        {
            if (zoneInterest.TryTakeEmptyPlace(Character, out placeInterest))
                break;
        }

        return placeInterest;
    }

    private void OnTimer—ompleted(Timer timer)
    {
        timer.Completed -= OnTimer—ompleted;
        Complete();
    }
}