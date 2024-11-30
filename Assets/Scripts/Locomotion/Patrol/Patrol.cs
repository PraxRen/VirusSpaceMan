using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(Mover), typeof(NavMeshAgent))]
public class Patrol : MonoBehaviour
{
    [SerializeField] private TargetTracker _moveTracker;

    private Transform _transform;
    private Mover _mover;
    private NavMeshAgent _navMeshAgent;
    private PatrolPath _patrolPath;
    private int _indexWaypoint;

    private void Awake()
    {
        _transform = transform;
        _mover = GetComponent<Mover>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _indexWaypoint = -1;
    }

    private void OnDisable()
    {
        Clear();
    }

    public void ResetPatrolPath(PatrolPath patrolPath, int index = 0)
    {
        _patrolPath = patrolPath;
        _indexWaypoint = index;
    }

    public void Run()
    {
        if (_patrolPath == null)
            return;

        StartCoroutine(UpdateMover());
    }

    public void Clear()
    {
        _patrolPath = null;
    }

    private IEnumerator UpdateMover()
    {
        while (_patrolPath != null) 
        {
            Waypoint waypoint = _patrolPath.GetWaypoint(_indexWaypoint);
            Vector2 direction = Navigation.CalculateDirectionVector2(_navMeshAgent, _transform, waypoint.Position);
            _moveTracker.SetTarget(waypoint, Vector3.zero);
            _mover.Move(direction);
            _mover.LookAtDirection(direction);

            if (waypoint.CanReach(_transform))
                _patrolPath.SetNextIndex(ref _indexWaypoint);
            
            yield return null;
        }
    }
}