using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Storage<T> : MonoBehaviour, IReadOnlyStorage<T> where T : IObjectItem
{
#if UNITY_EDITOR
    [SerializeField][ReadOnly] private List<string> _idsDebug;
#endif
    [SerializeField] private ScriptableObject _slotFactoryScriptableObject;

    private ISlotFactory<T> _slotFactory;
    private List<BaseSlot<T>> _slots;

    public event Action<IReadOnlySlot<T>, T> AddedItem;
    public event Action<IReadOnlySlot<T>, T> RemovedItem;
    public event Action Initialized;

    public IReadOnlyList<IReadOnlySlot<T>> Slots => _slots;

    private void OnValidate()
    {
        if (_slotFactoryScriptableObject == null)
            return;

        _slotFactory = _slotFactoryScriptableObject as ISlotFactory<T>;

        if (_slotFactory == null)
        {
            _slotFactoryScriptableObject = null;
            Debug.LogWarning($"{nameof(_slotFactoryScriptableObject)} is not {nameof(ISlotFactory<T>)}");
        }
    }

    private void Awake()
    {
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
        AwakeStart();
    }

    public void Initialize(IEnumerable<IDataSlot<T>> dataItems)
    {
        if (dataItems == null)
            throw new ArgumentNullException(nameof(dataItems));

        if (dataItems.Count() == 0)
            throw new ArgumentOutOfRangeException(nameof(dataItems));

        _slots = new List<BaseSlot<T>>();

        foreach (IDataSlot<T> dataItem in dataItems)
        {
            BaseSlot<T> slot = _slotFactory.Create(dataItem, this);
            _slots.Add(slot);
            
            if (slot.Item != null)
            {
#if UNITY_EDITOR
                _idsDebug.Add(slot.Item.Id);
#endif
                AddedItem?.Invoke(slot, slot.Item);
            }
        }

        Initialized?.Invoke();
    }

    public bool TryAddItem(IReadOnlySlot<T> slot, T item, int count)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        BaseSlot<T> foundSlot = _slots.FirstOrDefault(insideSlot => slot.Id == insideSlot.Id);

        if (foundSlot == null)
            return false;

        if (foundSlot.TryAddItem(item, count) == false)
            return false;

#if UNITY_EDITOR
        _idsDebug.Add(item.Id);
#endif
        AddedItem?.Invoke(foundSlot, item);
        return true;
    }

    public bool TryAddItem(T item, int count)
    {
        BaseSlot<T> slot = _slots.FirstOrDefault(slot => slot.IsEmpty == false && slot.Item.Id == item.Id) ?? _slots.FirstOrDefault(slot => slot.IsEmpty);

        if (slot == null)
            return false;

        return TryAddItem(slot, item, count);
    }

    public bool TryRemoveItem(IReadOnlySlot<T> slot, int count)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        BaseSlot<T> foundSlot = _slots.FirstOrDefault(insideSlot => slot.Id == insideSlot.Id);

        if (foundSlot == null)
            return false;

        T item = foundSlot.Item;

        if (foundSlot.TryRemoveItem(count) == false)
            return false;

#if UNITY_EDITOR
        _idsDebug.Remove(item.Id);
#endif
        RemovedItem?.Invoke(foundSlot, item);
        return true;
    }

    public bool TryRemoveItem(T item, int count)
    {
        BaseSlot<T> slot = _slots.FirstOrDefault(slot => slot.Item.Id == item.Id);

        if (slot == null)
            return false;

        return TryRemoveItem(slot, count);
    }

    public bool TryGiveItem(out T item, IReadOnlySlot<T> slot, int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        item = default;
        BaseSlot<T> foundSlot = _slots.FirstOrDefault(insideSlot => insideSlot.Id == slot.Id);

        if (foundSlot == null)
            return false;

        return foundSlot.TryGiveItem(out item, count);
    }

    public bool HasItem(T item) => _slots.Any(slot => slot.Item.Id == item.Id);

    protected virtual void AwakeAddon() { }

    protected virtual void AwakeStart() { }

    protected virtual void EnableAddon() { }

    protected virtual void DisableAddon() { }
}