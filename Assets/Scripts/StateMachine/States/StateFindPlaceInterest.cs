using System;
using System.Linq;
using UnityEngine;

public class StateFindPlaceInterest : State
{
    [SerializeField] private TargetTracker _moveTracker;

    private HandlerZoneEnvironment _handlerZoneEnvironment;

    protected override void InitializeAfterAddon()
    {
        if (Character.TryGetComponent(out _handlerZoneEnvironment) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(HandlerZoneEnvironment)}\" required for operation \"{GetType().Name}\".");
    }

    protected override void HandleAddon()
    {
        IReadOnlyPlaceInterest currentPlaceInterest = FindPlaceInterest();

        if (currentPlaceInterest != null)
        {
            _moveTracker.SetTarget(currentPlaceInterest, Vector3.zero);
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