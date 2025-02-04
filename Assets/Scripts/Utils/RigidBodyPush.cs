using UnityEngine;

public class RigidBodyPush : MonoBehaviour
{
    [SerializeField] private Vector3 _offsetStartPoint;
    [SerializeField] private float _height;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private float _factorDirectionY;
    [SerializeField] private float _force;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_startPosition, _radius);
        Gizmos.DrawWireSphere(_endPosition, _radius);
        Gizmos.DrawLine(_startPosition + Vector3.up * _radius, _endPosition + Vector3.up * _radius);
        Gizmos.DrawLine(_startPosition - Vector3.up * _radius, _endPosition - Vector3.up * _radius);
        Gizmos.DrawLine(_startPosition + Vector3.right * _radius, _endPosition + Vector3.right * _radius);
        Gizmos.DrawLine(_startPosition - Vector3.right * _radius, _endPosition - Vector3.right * _radius);
        Gizmos.DrawLine(_startPosition + Vector3.forward * _radius, _endPosition + Vector3.forward * _radius);
        Gizmos.DrawLine(_startPosition - Vector3.forward * _radius, _endPosition - Vector3.forward * _radius);
    }

    private void FixedUpdate()
    {
        UpdatePositions();

        RaycastHit[] hits = Physics.CapsuleCastAll(_startPosition, _endPosition, _radius, Vector3.up, 0f, _layerMask);

        foreach (RaycastHit hit in hits) 
        {
            Rigidbody hitRigidbody = hit.rigidbody;

            if (hitRigidbody == null || hitRigidbody.isKinematic)
                continue;

            if (SimpleUtils.IsLayerInclud(hitRigidbody.gameObject.layer, _layerMask) == false)
                continue;

            Vector3 directionForce = (hit.point - _transform.position);
            directionForce.y = _factorDirectionY;
            hitRigidbody.AddForceAtPosition(directionForce.normalized * _force, hit.point, ForceMode.Impulse);
        }
    }

    private void UpdatePositions()
    {
        _startPosition = _transform.position + _offsetStartPoint;
        _endPosition = _startPosition + Vector3.up * _height;
    }
}