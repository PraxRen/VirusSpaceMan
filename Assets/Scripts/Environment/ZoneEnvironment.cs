using System.Collections.Generic;
using UnityEngine;

public class ZoneEnvironment : MonoBehaviour
{
    [SerializeField] private ZoneInterest[] _interests;
    [SerializeField] private PatrolPath[] _patrolPaths;

    public IReadOnlyList<ZoneInterest> Interests => _interests;
    public IReadOnlyList<PatrolPath> PatrolPaths => _patrolPaths;
}