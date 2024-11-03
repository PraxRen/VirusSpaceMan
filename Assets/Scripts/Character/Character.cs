using UnityEngine;

[RequireComponent(typeof(IMover))]
public abstract class Character : MonoBehaviour
{
    private IMover _mover;

    protected IMover Mover => _mover;
    
    private void Awake()
    {
        _mover = GetComponent<IMover>();
        AwakeAddon();
    }

    private void Start()
    {
        StartAddon();
    }

    private void OnEnable()
    {
        EnableAddon();
    }

    private void OnDisable()
    {
        DisableAddon();
    }

    protected virtual void AwakeAddon() { }

    protected virtual void StartAddon() { }

    protected virtual void EnableAddon() { }

    protected virtual void DisableAddon() { }
}