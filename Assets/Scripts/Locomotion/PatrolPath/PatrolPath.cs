using UnityEngine;
using System;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] private Waypoint[] _waypoints;
    [SerializeField] private bool _isOneWay;
#if UNITY_EDITOR
    [Header("Gizmos")]
    [SerializeField] private Color _color;
#endif

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        int nextIndex = 0;

        for (int i = 0; i < _waypoints.Length; i++)
        {
            nextIndex = i + 1;
            nextIndex = nextIndex == _waypoints.Length ? 0 : nextIndex;

            Gizmos.DrawWireSphere(_waypoints[i].transform.position, _waypoints[i].Radius);
            Gizmos.DrawLine(_waypoints[i].transform.position, _waypoints[nextIndex].transform.position);
        }
    }

    public void SetNextIndex(ref int currentIndex)
    {
        currentIndex++;
        currentIndex = currentIndex >= _waypoints.Length ? 0 : currentIndex;
    }

    public Waypoint GetWaypoint(int index)
    {
        if (index >= _waypoints.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _waypoints[index];
    }

#if UNITY_EDITOR
    [ContextMenu("Find Waypoints")]
    private void FindWaypoints()
    {
        _waypoints = GetComponentsInChildren<Waypoint>();
    }
#endif
}