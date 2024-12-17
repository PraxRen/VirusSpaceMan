using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Mover), typeof(NavMeshAgent))]
public class Patrol : MonoBehaviour, IReadOnlyPatrol
{
    [SerializeField] private TargetTracker _moveTracker;

    private Transform _transform;
    private Mover _mover;
    private NavMeshAgent _navMeshAgent;
    private PatrolPath _patrolPath;
    private int _indexWaypoint;

    public float LastTimeChangeIndex { get; private set; }
    public Waypoint LastWaypoint { get; private set; }

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
            throw new InvalidOperationException($"The current {nameof(PatrolPath)} is not initialized");

        StartCoroutine(UpdateMover());
    }

    public void Clear()
    {
        _patrolPath = null;
        _indexWaypoint = -1;
    }

    private IEnumerator UpdateMover()
    {

        LastWaypoint = _patrolPath.GetWaypoint(_indexWaypoint);
        _moveTracker.SetTarget(LastWaypoint, Vector3.zero);

        while (_patrolPath != null) 
        {
            Vector2 direction = Navigation.CalculateDirectionVector2(_navMeshAgent, _transform, _moveTracker.Target.Position);
            _mover.Move(direction);
            _mover.LookAtDirection(direction);

            if (LastWaypoint.CanReach(_transform))
            {
                _patrolPath.SetNextIndex(ref _indexWaypoint);
                LastWaypoint = _patrolPath.GetWaypoint(_indexWaypoint);
                _moveTracker.SetTarget(LastWaypoint, Vector3.zero);
                LastTimeChangeIndex = Time.time;
            }
            
            yield return null;
        }
    }
}