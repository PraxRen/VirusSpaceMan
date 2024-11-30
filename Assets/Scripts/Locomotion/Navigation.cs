using UnityEngine;
using UnityEngine.AI;

public class Navigation
{
    public static Vector3 CalculateDirection(NavMeshAgent navMeshAgent, Transform transform, Vector3 target)
    {
        if (navMeshAgent.enabled == false)
            return Vector3.zero;

        if (navMeshAgent.isOnNavMesh == false)
            return Vector3.zero;

        navMeshAgent.SetDestination(target);

        if (navMeshAgent.pathPending)
            return Vector3.zero;

        Vector3 direction = navMeshAgent.desiredVelocity.normalized;
        navMeshAgent.nextPosition = transform.position;
        return direction;
    }

    public static Vector2 CalculateDirectionVector2(NavMeshAgent navMeshAgent, Transform transform, Vector3 target)
    {
        Vector3 direction = CalculateDirection(navMeshAgent, transform, target);
        return new Vector2(direction.x, direction.z);
    }

    public static void ResetNavMeshAgent(NavMeshAgent navMeshAgent)
    {
        if (navMeshAgent.enabled == false)
            return;

        if (navMeshAgent.isOnNavMesh == false)
            return;

        navMeshAgent.ResetPath();
        navMeshAgent.velocity = Vector3.zero;
    }
}