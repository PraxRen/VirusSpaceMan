using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(IMover))]
public abstract class Character : MonoBehaviour
{
    [SerializeField] private LookTarget _lookTarget;

    private IMover _mover;

    protected Transform Transform { get; private set; }
    protected IMover Mover => _mover;
    protected LookTarget LookTarget => _lookTarget;
    
    private void Awake()
    {
        Transform = transform;
        _mover = GetComponent<IMover>();
        AwakeAddon();
    }

    private void OnEnable()
    {
        if (_lookTarget.gameObject.activeSelf == false)
            _lookTarget.gameObject.SetActive(true);

        EnableAddon();
    }

    private void OnDisable()
    {
        if (_lookTarget != null && _lookTarget.gameObject.activeSelf)
            _lookTarget.gameObject.SetActive(false);

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