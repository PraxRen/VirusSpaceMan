using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(IMover))]
public abstract class Character : MonoBehaviour
{
    private IMover _mover;

    protected Transform Transform { get; private set; }
    protected IMover Mover => _mover;
    
    private void Awake()
    {
        Transform = transform;
        _mover = GetComponent<IMover>();
        AwakeAddon();
    }

    private void OnEnable()
    {
        EnableAddon();
    }

    private void OnDisable()
    {
        DisableAddon();
    }

    private void Start()
    {
        StartAddon();
    }

    protected virtual void AwakeAddon() { }

    protected virtual void EnableAddon() { }

    protected virtual void DisableAddon() { }

    protected virtual void StartAddon() { }
}