using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMoverToTarget : State
{
    private Navigation _navigation;
    private HandlerZoneEnvironment _handlerZoneEnvironment;
    private List<ZoneInterest> _zoneInterests;
    private IReadOnlyPlaceInterest _currentPlaceInterest;

    protected override void InitializeAfterAddon()
    {
        if (Character.TryGetComponent(out _navigation) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(Navigation)}\" required for operation \"{GetType().Name}\".");

        if (Character.TryGetComponent(out _handlerZoneEnvironment) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(HandlerZoneEnvironment)}\" required for operation \"{GetType().Name}\".");
    }

    protected override void EnterAfterAddon()
    {
        _navigation.ResetPath();
        _handlerZoneEnvironment.ChangedZone += ResetZoneInterest;
        ResetZoneInterest(_handlerZoneEnvironment.CurrentZone);
    }

    protected override void ExitAfterAddon()
    {
        _handlerZoneEnvironment.ChangedZone -= ResetZoneInterest;
        _zoneInterests = null;
        _currentPlaceInterest = null;
    }

    protected override bool CanHandleAddon()
    {
        return _currentPlaceInterest != null && _currentPlaceInterest.HasCharacterInside == false;
    }

    protected override void HandleAddon()
    {
        _navigation.MoveTargetPosition(_currentPlaceInterest.Position);
    }

    private void ResetZoneInterest(ZoneEnvironment zoneEnvironment)
    {
        if (zoneEnvironment.Interests == null)
            return;

        _zoneInterests = zoneEnvironment.Interests.Where(zoneInterest => zoneInterest.gameObject.activeSelf)
                                                  .OrderBy(zoneInterest => (zoneInterest.Transform.position - Character.Transform.position).sqrMagnitude)
                                                  .ToList();

        foreach (ZoneInterest zoneInterest in _zoneInterests)
        {
            if (zoneInterest.TryTakeEmptyPlace(Character, out _currentPlaceInterest))
                break;
        }
    }
}