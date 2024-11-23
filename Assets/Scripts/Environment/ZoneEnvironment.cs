using System.Collections.Generic;
using UnityEngine;

public class ZoneEnvironment : MonoBehaviour
{
    [SerializeField] private ZoneInterest[] _interests;

    public IReadOnlyList<ZoneInterest> Interests => _interests;
}