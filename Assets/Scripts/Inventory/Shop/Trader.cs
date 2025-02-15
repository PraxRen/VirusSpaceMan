using System;
using UnityEngine;

public abstract class Trader<T> : MonoBehaviour, IReadOnlyTrader where T : IObjectItem
{
    [SerializeField] private MonoBehaviour _storageMonoBehaviour;

    public ISimpleStorage SimpleStorage => Storage;
    protected Storage<T> Storage { get; private set; }

    public event Action<ISimpleStorage> Changed;

    private void Awake()
    {
        Storage = (Storage<T>)_storageMonoBehaviour;
    }

    private void OnEnable()
    {
        Storage.Initialized += OnStorageChanged;
    }

    private void OnDisable()
    {
        Storage.Initialized -= OnStorageChanged;
    }

    private void OnStorageChanged()
    {
        Changed?.Invoke(SimpleStorage);
    }
}