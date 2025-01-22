using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StorageMonoBehaviour<T> : MonoBehaviour, IStorage<T> where T : IObjectItem
{
#if UNITY_EDITOR
    [SerializeField][ReadOnly] private List<string> _idsDebug;
#endif
    private Storage<T> _storage;

    public event Action<IReadOnlySlot<T>, T> AddedItem;
    public event Action<IReadOnlySlot<T>, T> RemovedItem;

    public int LimitSlots => _storage.LimitSlots;
    public IReadOnlyList<IReadOnlySlot<T>> Slots => _storage.Slots;

    private void Awake()
    {
        _storage = new Storage<T>();
        AwakeAddon();
    }

    private void OnEnable()
    {
        _storage.AddedItem += OnAddedItem;
        _storage.RemovedItem += OnRemovedItem;
        EnableAddon();
    }

    private void OnDisable()
    {
        _storage.AddedItem -= OnAddedItem;
        _storage.RemovedItem -= OnRemovedItem;
        DisableAddon();
    }

    private void Start()
    {
        _storage.Initilize(CreateSlots());
        AwakeStart();
    }

    public bool TryAddItem(IReadOnlySlot<T> slot, T item, int count) => _storage.TryAddItem(slot, item, count);

    public bool TryAddItem(T item, int count) => _storage.TryAddItem(item, count);

    public bool TryRemoveItem(IReadOnlySlot<T> slot, int count) => _storage.TryRemoveItem(slot, count);

    public bool TryRemoveItem(T item, int count) => _storage.TryRemoveItem(item, count);

    public bool TryGiveItem(out T item, IReadOnlySlot<T> slot, int count) => _storage.TryGiveItem(out item, slot, count);

    public bool HasItem(T item) => _storage.HasItem(item);

    protected abstract IEnumerable<BaseSlot<T>> CreateSlots();

    protected virtual void AwakeAddon() { }

    protected virtual void AwakeStart() { }

    protected virtual void EnableAddon() { }

    protected virtual void DisableAddon() { }

    protected virtual void HandleAddedItem(IReadOnlySlot<T> slot, T item) { }

    protected virtual void HandlenRemovedItem(IReadOnlySlot<T> slot, T item) { }

    private void OnRemovedItem(IReadOnlySlot<T> slot, T item)
    {
        HandlenRemovedItem(slot, item);
        RemovedItem?.Invoke(slot, item);
#if UNITY_EDITOR
        _idsDebug.Remove(item.Id);
#endif
    }

    private void OnAddedItem(IReadOnlySlot<T> slot, T item)
    {
        HandleAddedItem(slot, item);
        AddedItem?.Invoke(slot, item);
#if UNITY_EDITOR
        _idsDebug.Add(item.Id);
#endif
    }
}