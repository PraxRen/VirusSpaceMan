using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Equipment : MonoBehaviour, IEquipmentReadOnly
{
    [SerializeField] private EquipmentCell[] _defaultCells;

    private EquipmentCell[] _equipmentCells;

    public event Action<IEquipmentCellReanOnly> ChangedCell;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        foreach (EquipmentCell cell in _defaultCells) 
        {
            UpdateCell(cell.Type, cell.Item);
        }
    }

    public void UpdateCell(EquipmentType type, Item item)
    {
        EquipmentCell equipmentCell = _equipmentCells.FirstOrDefault(equipmentCell => equipmentCell.Type == type);

        if (equipmentCell == null)
            throw new ArgumentNullException(nameof(equipmentCell));

        equipmentCell.AddItem(item);
        ChangedCell?.Invoke(equipmentCell);
    }

    private void Init()
    {
        EquipmentType[] types = (EquipmentType[])Enum.GetValues(typeof(EquipmentType));
        _equipmentCells = new EquipmentCell[types.Length];

        for (int i = 0; i < types.Length; i++) 
        {
            _equipmentCells[i] = new EquipmentCell(types[i], null);
        }
    }
}