using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DisplayerStorage<T> : MonoBehaviour, IReadOnlyDisplayerStorage<T> where T : IObjectItem
{
    [SerializeField] private ScriptableObject _displayerSlotFactoryScriptableObject;
    
    private IDisplayerSlotFactory<T> _displayerSlotFactory;
    private List<IDisplayerSlot<T>> _displayerSlots = new List<IDisplayerSlot<T>>();
    private int _indexActiveSlot;

    public event Action<IReadOnlyDisplayerSlot<T>> ActiveDisplayerSlotChanged;

    public IReadOnlyDisplayerSlot<T> ActiveDisplayerSlot => _displayerSlots[_indexActiveSlot];
    public Transform Transform { get; private set; }

    private void OnValidate()
    {
        if (_displayerSlotFactoryScriptableObject != null)
        {
            _displayerSlotFactory = _displayerSlotFactoryScriptableObject as IDisplayerSlotFactory<T>;

            if (_displayerSlotFactory == null)
            {
                _displayerSlotFactoryScriptableObject = null;
                Debug.LogWarning($"{nameof(_displayerSlotFactoryScriptableObject)} is not {nameof(IDisplayerSlotFactory<T>)}");
            }
        }
    }

    private void Awake()
    {
        Transform = transform;
    }

    public void Next()
    {
        _indexActiveSlot = _indexActiveSlot == _displayerSlots.Count - 1 ? 0 : _indexActiveSlot + 1;
        ActiveDisplayerSlotChanged?.Invoke(ActiveDisplayerSlot);
    }

    public void Previous()
    {
        _indexActiveSlot = _indexActiveSlot == 0 ? _displayerSlots.Count - 1 : _indexActiveSlot - 1;
        ActiveDisplayerSlotChanged?.Invoke(ActiveDisplayerSlot);
    }

    public void Initialize(IReadOnlyStorage<T> storage)
    {
        foreach (IReadOnlySlot<T> slot in storage.Slots)
            _displayerSlots.Add(_displayerSlotFactory.Create(slot, this));

        _indexActiveSlot = 0;
        ActiveDisplayerSlotChanged?.Invoke(ActiveDisplayerSlot);
    }
}