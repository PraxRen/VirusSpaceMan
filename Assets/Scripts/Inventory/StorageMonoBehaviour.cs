using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StorageMonoBehaviour<T> : MonoBehaviour, IStorage<T> where T : IObjectItem
{
    private Storage<T> _storage;

    public event Action<IReadOnlySlot<T>> ChangedSlot;

    public int LimitSlots => _storage.LimitSlots;
    public IReadOnlyCollection<IReadOnlySlot<T>> Slots => _storage.Slots;

    private void Awake()
    {
        _storage = new Storage<T>(GetSlots());
        AwakeAddon();
    }

    private void OnEnable()
    {
        _storage.ChangedSlot += OnChangedSlot;
        EnableAddon();
    }

    private void OnDisable()
    {
        _storage.ChangedSlot -= OnChangedSlot;
        DisableAddon();
    }

    public bool TryAddItem(IReadOnlySlot<T> slot, T item, int count) => _storage.TryAddItem(slot, item, count);

    public bool TryAddItem(T item, int count) => _storage.TryAddItem(item, count);

    public bool TryRemoveItem(IReadOnlySlot<T> slot, int count) => _storage.TryRemoveItem(slot, count);

    public bool TryRemoveItem(T item, int count) => _storage.TryRemoveItem(item, count);

    public bool TryGiveItem(out T item, IReadOnlySlot<T> slot, int count) => _storage.TryGiveItem(out item, slot, count);

    public bool HasItem(T item) => _storage.HasItem(item);

    protected abstract IEnumerable<BaseSlot<T>> GetSlots();

    protected virtual void AwakeAddon() { }

    protected virtual void EnableAddon() { }

    protected virtual void DisableAddon() { }

    private void OnChangedSlot(IReadOnlySlot<T> slot) => ChangedSlot?.Invoke(slot);
}