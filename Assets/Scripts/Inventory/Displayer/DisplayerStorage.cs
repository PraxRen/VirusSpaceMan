using System;
using System.Collections.Generic;
using UnityEngine;

public class DisplayerStorage : MonoBehaviour, IReadOnlyDisplayerStorage
{
    [SerializeField][SerializeInterface(typeof(IDisplayerSlotFactory))] private ScriptableObject _displayerSlotFactoryScriptableObject;
    
    private IDisplayerSlotFactory _displayerSlotFactory;
    private List<IDisplayerSlot> _displayerSlots = new List<IDisplayerSlot>();
    private int _indexActiveSlot;

    public event Action<IReadOnlyDisplayerSlot> ActiveDisplayerSlotChanged;

    public IReadOnlyDisplayerSlot ActiveDisplayerSlot => _displayerSlots[_indexActiveSlot];
    public Transform Transform { get; private set; }

    private void Awake()
    {
        Transform = transform;
        _displayerSlotFactory = (IDisplayerSlotFactory)_displayerSlotFactoryScriptableObject;
    }

    public void Next()
    {
        int newIndex = _indexActiveSlot == _displayerSlots.Count - 1 ? 0 : _indexActiveSlot + 1;

        if (newIndex == _indexActiveSlot)
            return;

        _displayerSlots[_indexActiveSlot].Hide();
        _indexActiveSlot = newIndex;
        _displayerSlots[_indexActiveSlot].Show();
        ActiveDisplayerSlotChanged?.Invoke(ActiveDisplayerSlot);
    }

    public void Previous()
    {
        int newIndex = _indexActiveSlot == 0 ? _displayerSlots.Count - 1 : _indexActiveSlot - 1;

        if (newIndex == _indexActiveSlot)
            return;

        _displayerSlots[_indexActiveSlot].Hide();
        _indexActiveSlot = newIndex;
        _displayerSlots[_indexActiveSlot].Show();
        ActiveDisplayerSlotChanged?.Invoke(ActiveDisplayerSlot);
    }

    public void Initialize(ISimpleStorage simpleStorage)
    {
        foreach (ISimpleSlot slot in simpleStorage.GetSlots())
            _displayerSlots.Add(_displayerSlotFactory.Create(slot, this));

        _indexActiveSlot = 0;
        _displayerSlots[_indexActiveSlot].Show();
        ActiveDisplayerSlotChanged?.Invoke(ActiveDisplayerSlot);
    }
}