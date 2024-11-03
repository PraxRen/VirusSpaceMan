using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerModeMover : MonoBehaviour, IChangerModeMover
{
    [SerializeField][SerializeInterface(typeof(IEquipmentReadOnly))] private MonoBehaviour _equipmentMonoBehaviour;

    private IEquipmentReadOnly _equipment;

    public event Action<IModeMoverProvider> ChangedModeMover;

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
        if (equipmentCell.Type == EquipmentType.Weapon)
        {
            IModeMoverProvider modeMoverProvider = equipmentCell.Item as IModeMoverProvider;
            ChangedModeMover?.Invoke(modeMoverProvider);
        }
    }
}
