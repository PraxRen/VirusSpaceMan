using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private Vector3 _factor;
    [SerializeField] private Space _space;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.Rotate(_factor * Time.deltaTime, _space);
    }
}