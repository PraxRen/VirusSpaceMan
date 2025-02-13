using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisplayerStorage : MonoBehaviour, IReadOnlyDisplayerStorage
{
    [SerializeField][SerializeInterface(typeof(IDisplayerSlotFactory))] private ScriptableObject _displayerSlotFactoryScriptableObject;
    
    private IDisplayerSlotFactory _displayerSlotFactory;
    private List<IDisplayerSlot> _displayerSlots = new List<IDisplayerSlot>();

    public Transform Transform { get; private set; }

    private void Awake()
    {
        Transform = transform;
        _displayerSlotFactory = (IDisplayerSlotFactory)_displayerSlotFactoryScriptableObject;
    }

    public void Show(ISimpleSlot simpleSlot)
    {
        IDisplayerSlot slot = _displayerSlots.FirstOrDefault(slot => slot.Slot.Id == simpleSlot.Id);

        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        slot.Show();
    }

    public void Hide(ISimpleSlot simpleSlot)
    {
        IDisplayerSlot slot = _displayerSlots.FirstOrDefault(slot => slot.Slot.Id == simpleSlot.Id);

        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        slot.Hide();
    }

    public void Initialize(ISimpleStorage simpleStorage)
    {
        foreach (IDisplayerSlot displayerSlot in _displayerSlots.ToArray())
        {
            displayerSlot.Destroy();
            _displayerSlots.Remove(displayerSlot);
        }

        foreach (ISimpleSlot slot in simpleStorage.GetSlots())
        {
            if (slot.IsEmpty)
                continue;

            _displayerSlots.Add(_displayerSlotFactory.Create(slot, this));
        }
    }
}