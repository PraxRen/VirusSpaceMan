using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] private List<Waypoint> _waypoints;
#if UNITY_EDITOR
    [Header("Gizmos")]
    [SerializeField] private Color _color;
#endif

    private void Awake()
    {
        foreach (var waypoint in _waypoints) 
        {
            waypoint.Initialize(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        int nextIndex = 0;

        for (int i = 0; i < _waypoints.Count; i++)
        {
            nextIndex = i + 1;
            nextIndex = nextIndex == _waypoints.Count ? 0 : nextIndex;

            Gizmos.DrawWireSphere(_waypoints[i].transform.position, _waypoints[i].Radius);
            Gizmos.DrawLine(_waypoints[i].transform.position, _waypoints[nextIndex].transform.position);
        }
    }

    public void SetNextIndex(ref int currentIndex)
    {
        currentIndex++;
        currentIndex = currentIndex >= _waypoints.Count ? 0 : currentIndex;
    }

    public Waypoint GetWaypoint(int index)
    {
        if (index >= _waypoints.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _waypoints[index];
    }

    public Waypoint GetNearestWaypoint(Vector3 position, out int index) 
    {
        index = -1;
        Waypoint result = _waypoints.OrderBy(waypoint => (waypoint.Position - position).sqrMagnitude).First();
        index = _waypoints.IndexOf(result);

        return result;
    }

#if UNITY_EDITOR
    [ContextMenu("Find Waypoints")]
    private void FindWaypoints()
    {
        _waypoints = GetComponentsInChildren<Waypoint>()?.ToList();
    }
#endif
}