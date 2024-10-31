using UnityEngine;

public class RotationDebug : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void Update()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        direction.y = 0f;
        transform.forward = direction;
    }
}
