using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public abstract class Character : MonoBehaviour, IReadOnlyCharacter
{
    [SerializeField][ReadOnly] private string _id;
    [SerializeField] private TargetTracker _lookTracker;
    [SerializeField] private TargetTracker _moveTracker;

    public string Id => _id;
    public Transform Transform { get; private set; }
    protected TargetTracker LookTracker => _lookTracker;
    protected TargetTracker MoveTracker => _moveTracker;
    protected Mover Mover { get; private set; }
    protected Scanner Scanner { get; private set; }
    protected Fighter Fighter { get; private set; }

    private void Awake()
    {
        if (string.IsNullOrWhiteSpace(_id))
            _id = Guid.NewGuid().ToString();

        Transform = transform;
        Mover = GetComponent<Mover>();
        Fighter = GetComponent<Fighter>();
        Scanner = GetComponent<Scanner>();
        AwakeAddon();
    }

    private void OnEnable()
    {
        if (_lookTracker.gameObject.activeSelf == false)
            _lookTracker.gameObject.SetActive(true);

        if (_moveTracker.gameObject.activeSelf == false)
            _moveTracker.gameObject.SetActive(true);

        Mover.enabled = true;
        Fighter.enabled = true;
        Scanner.enabled = true;
        EnableAddon();
    }

    private void OnDisable()
    {
        if (_lookTracker != null && _lookTracker.gameObject.activeSelf)
            _lookTracker.gameObject.SetActive(false);

        if (_moveTracker != null && _moveTracker.gameObject.activeSelf)
            _moveTracker.gameObject.SetActive(false);

        Mover.enabled = false;
        Fighter.enabled = false;
        Scanner.enabled = false;
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