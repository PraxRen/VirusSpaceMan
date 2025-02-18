using System;
using UnityEngine;

public class ListenerEquipmentItem : MonoBehaviour, IChangerWeaponConfig, IChangerArmorConfig
{
    [SerializeField][SerializeInterface(typeof(IReadOnlyStorage<IEquipmentItem>))] private MonoBehaviour _equipmentMonoBehaviour;

    private IReadOnlyStorage<IEquipmentItem> _equipment;

    public event Action<IWeaponConfig> ChangedWeaponConfig;
    public event Action RemovedWeaponConfig;
    public event Action<IArmorConfig> ChangedArmorConfig;
    public event Action RemovedArmorConfig;

    private void Awake()
    {
        _equipment = (IReadOnlyStorage<IEquipmentItem>)_equipmentMonoBehaviour;
    }

    private void OnEnable()
    {
        _equipment.AddedItem += OnAddedItem;
        _equipment.RemovedItem += OnRemovedItem;
    }

    private void OnDisable()
    {
        _equipment.AddedItem -= OnAddedItem;
        _equipment.RemovedItem -= OnRemovedItem;
    }

    private void OnRemovedItem(IReadOnlySlot<IEquipmentItem> slot, IEquipmentItem item)
    {
        EquipmentType equipmentType = slot.Item.Type;

        if (equipmentType == EquipmentType.Weapon)
        {
            RemovedWeaponConfig?.Invoke();
            return;
        }

        if (equipmentType == EquipmentType.Armor)
        {
            RemovedArmorConfig?.Invoke();
        }

    }

    private void OnAddedItem(IReadOnlySlot<IEquipmentItem> slot, IEquipmentItem item)
    {
        EquipmentType equipmentType = slot.Item.Type;

        if (equipmentType == EquipmentType.Weapon)
        {
            ChangedWeaponConfig?.Invoke((IWeaponConfig)item);
            return;
        }

        if (equipmentType == EquipmentType.Armor)
        {
            ChangedArmorConfig?.Invoke((IArmorConfig)item);
        }
    }
}