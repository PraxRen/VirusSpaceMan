using System;
using System.Linq;
using UnityEngine;

public class StateSearchPlaceInterest : State
{
    private HandlerZoneEnvironment _handlerZoneEnvironment;

    public StateSearchPlaceInterest(string id, Character character, float timeSecondsWaitHandle) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _handlerZoneEnvironment) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(HandlerZoneEnvironment)}\" required for operation \"{GetType().Name}\".");
    }

    public override void Update()
    {
        IReadOnlyPlaceInterest currentPlaceInterest = FindPlaceInterest();

        if (currentPlaceInterest != null)
        {
            Character.MoveTracker.SetTarget(currentPlaceInterest, Vector3.zero);
            Complete();
        }
    }

    private IReadOnlyPlaceInterest FindPlaceInterest()
    {
        IReadOnlyPlaceInterest currentPlaceInterest = null;

        if (_handlerZoneEnvironment.CurrentZone.Interests == null)
            return null;

        var zoneInterests = _handlerZoneEnvironment.CurrentZone.Interests.Where(zoneInterest => zoneInterest.gameObject.activeSelf)
                                                                         .OrderBy(zoneInterest => (zoneInterest.Transform.position - Character.Transform.position).sqrMagnitude);

        foreach (ZoneInterest zoneInterest in zoneInterests)
        {
            if (zoneInterest.TryTakeEmptyPlace(Character, out currentPlaceInterest))
                break;
        }

        return currentPlaceInterest;
    }
}