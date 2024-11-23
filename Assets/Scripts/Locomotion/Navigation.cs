using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private Transform _transfrom;

    private void OnValidate()
    {
        if (_navMeshAgent == null)
            return;

        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
    }

    private void Awake()
    {
        _transfrom = transform;
    }

    public void ResetPath()
    {
        _navMeshAgent.ResetPath();
        _navMeshAgent.velocity = Vector3.zero;
    }

    public void MoveTargetPosition(Vector3 position)
    {
        if (_navMeshAgent.enabled == false)
            return;

        if (_navMeshAgent.isOnNavMesh == false)
            return;

        _navMeshAgent.SetDestination(position);

        if (_navMeshAgent.pathPending)
            return;

        Vector3 desiredVelocity = _navMeshAgent.desiredVelocity;
        desiredVelocity.y = 0;
        desiredVelocity.Normalize();
        Vector2 direction = new Vector2(desiredVelocity.x, desiredVelocity.z);
        _mover.Move(direction);
        _mover.LookAtDirection(direction);
        _navMeshAgent.nextPosition = _transfrom.position;
    }

    public void Stop()
    {
        if (_navMeshAgent.enabled == false)
            return;

        if (_navMeshAgent.isOnNavMesh == false)
            return;

        _mover.Cancel();
        ResetPath();
    }
}
