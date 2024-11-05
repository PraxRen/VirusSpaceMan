using System;
using UnityEngine;

public class ChangerWeaponConfig : MonoBehaviour, IChangerWeaponConfig
{
    [SerializeField][SerializeInterface(typeof(IEquipmentReadOnly))] private MonoBehaviour _equipmentMonoBehaviour;

    private IEquipmentReadOnly _equipment;

    public event Action<WeaponConfig> ChangedWeaponConfig;
    public event Action RemovedWeaponConfig;

    private void Awake()
    {
        _equipment = (IEquipmentReadOnly)_equipmentMonoBehaviour;
    }

    private void OnEnable()
    {
        _equipment.ChangedCell += OnChangedCell;
    }

    private void OnDisable()
    {
        _equipment.ChangedCell -= OnChangedCell;
    }

    private void OnChangedCell(IEquipmentCellReanOnly equipmentCell)
    {
        if (equipmentCell.Type != EquipmentType.Weapon)
            return;

        WeaponConfig weaponConfig = equipmentCell.Item as WeaponConfig;

        if (weaponConfig == null)
        {
            RemovedWeaponConfig?.Invoke();
            return;
        }

        ChangedWeaponConfig?.Invoke(weaponConfig);
    }
}