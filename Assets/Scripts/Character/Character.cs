using System;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Fighter), typeof(Scanner))]
[RequireComponent(typeof(Interactor))]
public abstract class Character : MonoBehaviour, IReadOnlyCharacter
{
    [SerializeField][ReadOnly] private string _id;
    [SerializeField] private TargetTracker _lookTracker;
    [SerializeField] private TargetTracker _moveTracker;
    [SerializeField] private Scanner _scannerDamageable;
    [SerializeField][SerializeInterface(typeof(IHealth))] private MonoBehaviour _healthMonoBehaviour;

    public string Id => _id;
    public Transform Transform { get; private set; }
    public IHealth Health { get; private set; }
    public TargetTracker LookTracker => _lookTracker;
    public TargetTracker MoveTracker => _moveTracker;
    public Scanner ScannerDamageable => _scannerDamageable;
    protected Mover Mover { get; private set; }
    protected Fighter Fighter { get; private set; }
    protected Interactor Interactor { get; private set; }

    private void Awake()
    {
        if (string.IsNullOrWhiteSpace(_id))
            _id = Guid.NewGuid().ToString();

        Transform = transform;
        Health = (IHealth)_healthMonoBehaviour;
        Mover = GetComponent<Mover>();
        Fighter = GetComponent<Fighter>();
        Interactor = GetComponent<Interactor>();
        AwakeAddon();
    }

    private void OnEnable()
    {
        Fighter.ChangedWeapon += OnChangedWeapon;
        Fighter.RemovedWeapon += OnRemovedWeapon;

        if (Fighter.Weapon != null)
            OnChangedWeapon(Fighter.Weapon);

        if (_lookTracker.gameObject.activeSelf == false)
            _lookTracker.gameObject.SetActive(true);

        if (_moveTracker.gameObject.activeSelf == false)
            _moveTracker.gameObject.SetActive(true);

        Mover.enabled = true;
        Fighter.enabled = true;
        _scannerDamageable.enabled = true;
        Interactor.enabled = true;
        EnableAddon();
    }

    private void OnDisable()
    {
        Fighter.ChangedWeapon -= OnChangedWeapon;
        Fighter.RemovedWeapon -= OnRemovedWeapon;

        if (_lookTracker != null && _lookTracker.gameObject.activeSelf)
            _lookTracker.gameObject.SetActive(false);

        if (_moveTracker != null && _moveTracker.gameObject.activeSelf)
            _moveTracker.gameObject.SetActive(false);

        Mover.enabled = false;
        Fighter.enabled = false;
        _scannerDamageable.enabled = false;
        Interactor.enabled = false;
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

    private void OnChangedWeapon(IWeaponReadOnly weapon)
    {
        ScannerDamageable.StartScan(weapon.Config.DistanceAttack);
    }

    private void OnRemovedWeapon()
    {
        ScannerDamageable.ResetRadius();
    }
}