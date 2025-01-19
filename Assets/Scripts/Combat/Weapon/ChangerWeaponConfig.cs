using System;
using UnityEngine;

public class ChangerWeaponConfig : MonoBehaviour, IChangerWeaponConfig
{
    [SerializeField][SerializeInterface(typeof(IReadOnlyStorage<IEquipmentItem>))] private MonoBehaviour _equipmentMonoBehaviour;

    private IReadOnlyStorage<IEquipmentItem> _equipment;

    public event Action<IWeaponConfig> ChangedWeaponConfig;
    public event Action RemovedWeaponConfig;

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
        IReadOnlyEquipmentSlot equipmentSlot = (IReadOnlyEquipmentSlot)slot;

        if (equipmentSlot.Type != EquipmentType.Weapon)
            return;

        RemovedWeaponConfig?.Invoke();
    }

    private void OnAddedItem(IReadOnlySlot<IEquipmentItem> slot, IEquipmentItem item)
    {
        IReadOnlyEquipmentSlot equipmentSlot = (IReadOnlyEquipmentSlot)slot;

        if (equipmentSlot.Type != EquipmentType.Weapon)
            return;

        ChangedWeaponConfig?.Invoke((IWeaponConfig)item);
    }
}