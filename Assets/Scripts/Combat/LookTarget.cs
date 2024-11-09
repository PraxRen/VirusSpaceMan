using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LookTarget : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private Transform _pointDefault;
    [SerializeField] private float _speedUpdatePositionDefault;
    [SerializeField] private float _speedUpdatePositionTarget;

    private Transform _transform;
    private Transform _target;
    private Vector3 _offset;
    private float _speedUpdate;

    private void Awake()
    {
        _transform = transform;
        ResetPosition();
    }

    private void OnEnable()
    {
        if (_parent != null)
            ResetPosition();
    }

    public void Update()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _target.position + _offset, _speedUpdate * Time.deltaTime);
    }

    public void SetTarget(Transform transform, Vector3 offset)
    {
        _target = transform;
        _offset = offset;
        _speedUpdate = _speedUpdatePositionTarget;
    }

    public void ResetTarget()
    {
        _target = _pointDefault;
        _offset = Vector3.zero;
        _speedUpdate = _speedUpdatePositionDefault;
    }

    private void ResetPosition()
    {
        _transform.position = _pointDefault.position;
        _target = _pointDefault;
        _transform.parent = null;
    }
}