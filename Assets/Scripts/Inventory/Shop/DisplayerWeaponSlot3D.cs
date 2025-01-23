using System;
using UnityEngine;

public class DisplayerWeaponSlot3D : MonoBehaviour, IDisplayerSlot<IComplexWeaponConfig>
{
    private Transform _transfrom;
    private IComplexWeaponConfig _complexWeaponConfig;
    private Graphics _graphics;

    public event Action<IDisplayerSlot<IComplexWeaponConfig>> Selected;

    public IReadOnlySlot<IComplexWeaponConfig> Slot { get; private set; }

    private void Awake()
    {
        _transfrom = transform;
    }

    public void Initilize(IReadOnlySlot<IComplexWeaponConfig> slot)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        Slot = slot;
        Slot.AddedItem += OnAddedItem;
        Slot.RemovedItem += OnRemovedItem;
        _complexWeaponConfig = Slot.Item;
        _graphics = Instantiate(Slot.Item.PrefabGraphics, _transfrom);
    }

    private void OnAddedItem(IComplexWeaponConfig complexWeaponConfig, int count)
    {
        if (_complexWeaponConfig == complexWeaponConfig)
            return;
    }

    private void OnRemovedItem(IComplexWeaponConfig complexWeaponConfig, int count)
    {
        if (_complexWeaponConfig == complexWeaponConfig)
            return;
    }
}