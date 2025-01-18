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
        _equipment.ChangedSlot += OnChangedSlot;
    }

    private void OnDisable()
    {
        _equipment.ChangedSlot -= OnChangedSlot;
    }

    private void OnChangedSlot(IReadOnlySlot<IEquipmentItem> slot)
    {
        IReadOnlyEquipmentSlot equipmentSlot = slot as IReadOnlyEquipmentSlot;

        if (equipmentSlot == null)
        {
            Debug.Log("!!!!!!!!!!!!");
            return;
        }

        if (equipmentSlot.Type != EquipmentType.Weapon)
            return;

        if (equipmentSlot.IsEmpty)
        {
            RemovedWeaponConfig?.Invoke();
            return;
        }

        ChangedWeaponConfig?.Invoke((IWeaponConfig)equipmentSlot.Item);
    }
}