using System;
using System.Linq;
using UnityEngine;

public class StateSearchPatrolPath : State
{
    private const float FixTimeFindNewWaypoint = 2f;

    private IReadOnlyHandlerEnvironment _handlerEnvironment;
    private Patrol _patrol;
    private float _timeDelayComplete;
    private bool _isFoundPatrolPath;
    private Timer _timerDelayComplete;

    public StateSearchPatrolPath(string id, AICharacter character, float timeSecondsWaitHandle, float timeDelayComplete) : base(id, character, timeSecondsWaitHandle)
    {
        if (character.TryGetComponent(out _handlerEnvironment) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(HandlerEnvironment)}\" required for operation \"{GetType().Name}\".");

        if (character.TryGetComponent(out _patrol) == false)
            throw new InvalidOperationException($"Initialization error \"{nameof(State)}\"! The component \"{nameof(Patrol)}\" required for operation \"{GetType().Name}\".");

        _timeDelayComplete = timeDelayComplete;
        _timerDelayComplete = new Timer(_timeDelayComplete);
    }

    public override void Update()
    {
        if (_isFoundPatrolPath)
            return;

        PatrolPath patrolPath = FindPatrolPath(out int indexWaypoint);

        if (patrolPath == null)
            return;

        _isFoundPatrolPath = true;
        Character.MoveTracker.SetTarget(patrolPath.GetWaypoint(indexWaypoint), Vector3.zero);
        _patrol.ResetPatrolPath(patrolPath, indexWaypoint);
        _timerDelayComplete.Completed += OnTimerCompleted;
        AddTimer(_timerDelayComplete);
    }

    protected override void ExitAfterAddon()
    {
        _timerDelayComplete.Completed -= OnTimerCompleted;
        _timerDelayComplete.Reset(_timeDelayComplete);
        _isFoundPatrolPath = false;
    }

    private PatrolPath FindPatrolPath(out int indexWaypoint)
    {
        indexWaypoint = -1;

        if (_handlerEnvironment.CurrentZone.PatrolPaths == null)
            return null;

        var activatedPatrolPathes = _handlerEnvironment.CurrentZone.PatrolPaths.Where(zoneInterest => zoneInterest.gameObject.activeSelf);
        Waypoint nearestWaypoint = null;
        float minDistanceSqr = float.MaxValue;

        foreach (PatrolPath patrolPath in activatedPatrolPathes)
        {
            Waypoint waypoint = patrolPath.GetNearestWaypoint(Character.Transform.position, out indexWaypoint);
            float distanceSqr = (waypoint.Position - Character.Transform.position).sqrMagnitude;

            if (distanceSqr < minDistanceSqr)
            {
                minDistanceSqr = distanceSqr;
                nearestWaypoint = waypoint;
            }
        }

        if (nearestWaypoint == null)
            return null;

        if (_patrol.LastWaypoint != null && nearestWaypoint != _patrol.LastWaypoint)
        {
            if (nearestWaypoint.PatrolPath == _patrol.LastWaypoint.PatrolPath && (Time.time - _patrol.LastTimeChangeIndex) < FixTimeFindNewWaypoint)
            {
                nearestWaypoint = _patrol.LastWaypoint;
                indexWaypoint = nearestWaypoint.PatrolPath.GetIndex(nearestWaypoint);
            }

        }

        return nearestWaypoint.PatrolPath;
    }

    private void OnTimerCompleted(Timer timer)
    {
        timer.Completed -= OnTimerCompleted;
        Complete();
    }
}