using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Storage<T> : MonoBehaviour, ISaveable, IReadOnlyStorage<T> where T : IObjectItem
{
#if UNITY_EDITOR
    [SerializeField][ReadOnly] private int _countSlot;
    [SerializeField][ReadOnly] private int _countItems;
    [SerializeField][ReadOnly] private List<string> _idsDebug;
#endif
    [SerializeField] private ScriptableObject _slotFactoryScriptableObject;

    private static Dictionary<string, IObjectItem> _hashItems;

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
        _hashItems = GetHashItems();
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
#if UNITY_EDITOR
    private void Update()
    {
        _countSlot = _slots.Count;
        _countItems = _slots.Sum(slot => slot.Count);
    }
#endif

    public static T FindItemInHash(string idItem)
    {
        return _hashItems.ContainsKey(idItem) ? (T)_hashItems[idItem] : default;
    }

    public void Initialize(IEnumerable<DataSlot> dataSlots)
    {
        if (dataSlots == null)
            throw new ArgumentNullException(nameof(dataSlots));

        if (dataSlots.Count() == 0)
            throw new ArgumentOutOfRangeException(nameof(dataSlots));

        _slots = new List<BaseSlot<T>>();
#if UNITY_EDITOR
        _idsDebug.Clear();
#endif

        foreach (DataSlot dataSlot in dataSlots)
        {
            BaseSlot<T> slot = _slotFactory.Create(dataSlot, this);
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
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        BaseSlot<T> slot = _slots.FirstOrDefault(slot => slot.IsEmpty == false && slot.Item.Id == item.Id && slot.Count + count <= item.LimitInSlot) ?? _slots.FirstOrDefault(slot => slot.IsEmpty);

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

    public bool TryGetItem(out T item, Func<IReadOnlySlot<T>, bool> predicate)
    {
        IReadOnlySlot<T> slot = _slots.FirstOrDefault(predicate);
        item = slot.Item;

        return item != null;
    }

    public bool TryGiveItem(out T item, ISimpleSlot slot, int count)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        item = default;
        BaseSlot<T> foundSlot = _slots.FirstOrDefault(insideSlot => insideSlot.Id == slot.Id);

        if (foundSlot == null)
            return false;

        item = foundSlot.Item;

        if (TryRemoveItem(foundSlot, count) == false)
        {
            item = default;
            return false;
        }

        return true;
    }

    public bool HasItem(T item, out ISimpleSlot slot) 
    {
        slot = _slots.FirstOrDefault(slot => slot.IsEmpty == false && slot.Item.Id == item.Id);
        return slot != null;
    }

    public bool HasItem(string idItem, out ISimpleSlot slot)
    {
        slot = _slots.FirstOrDefault(slot => slot.IsEmpty == false && slot.Item.Id == idItem);
        return slot != null;
    }

    public bool TryFindSlot(out ISimpleSlot slot, Func<IReadOnlySlot<T>, bool> predicate)
    {
        slot = _slots.FirstOrDefault(predicate);
        return slot != null;
    }

    protected virtual void AwakeAddon() { }

    protected virtual void AwakeStart() { }

    protected virtual void EnableAddon() { }

    protected virtual void DisableAddon() { } 

    public IReadOnlyList<ISimpleSlot> GetSlots() => Slots;

    object ISaveable.CaptureState()
    {
        DataSlot[] dataSlots = new DataSlot[Slots.Count];

        for (int i = 0; i < dataSlots.Length; i++)
            dataSlots[i] = _slots[i].CreateDataSlot();

        return dataSlots;
    }

    void ISaveable.RestoreState(object state)
    {
        DataSlot[] dataSlots = state as DataSlot[];

        if (dataSlots == null)
            throw new InvalidCastException(nameof(state));

        Initialize(dataSlots);
    }

    private static Dictionary<string, IObjectItem> GetHashItems()
    {
        string path = "Items";
        IObjectItem[] items = Resources.LoadAll<Item>(path);

        return items.Length == 0 ? new Dictionary<string, IObjectItem>() : items.ToDictionary(item => item.Id);
    }
}