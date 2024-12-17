using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private ZoneInterest[] _interests;
    [SerializeField] private PatrolPath[] _patrolPaths;

    public IReadOnlyList<ZoneInterest> Interests => _interests;
    public IReadOnlyList<PatrolPath> PatrolPaths => _patrolPaths;

    public bool TryReserveNearestPlaceInterest(IReadOnlyInteractor handlerInteraction, out IReadOnlyPlaceInterest placeInterest)
    {
        placeInterest = null;
        var activeZoneInterests = _interests.Where(zoneInterest => zoneInterest.gameObject.activeSelf)
                                            .OrderBy(zoneInterest => (zoneInterest.Transform.position - handlerInteraction.Position).sqrMagnitude);

        foreach (ZoneInterest zoneInterest in activeZoneInterests)
        {
            if (zoneInterest.TryReserveEmptyPlace(handlerInteraction, out placeInterest))
                return true;
        }

        return false;
    }
}