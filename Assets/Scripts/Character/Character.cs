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

    protected virtual void AwakeAddon() { }
}
