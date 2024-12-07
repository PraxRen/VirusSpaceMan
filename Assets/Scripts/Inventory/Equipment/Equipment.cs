using System;
using System.Linq;
using UnityEngine;

public class Equipment : MonoBehaviour, IEquipmentReadOnly
{
    [SerializeField] private EquipmentCell[] _defaultCells;

    private EquipmentCell[] _equipmentCells;

    public event Action<IEquipmentCellReanOnly> ChangedCell;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        foreach (EquipmentCell cell in _equipmentCells) 
        {
            EquipmentCell equipmentCellDefault = _defaultCells.FirstOrDefault(defaultCell => defaultCell.Type == cell.Type);

            if (equipmentCellDefault != null)
                UpdateCell(cell.Type, equipmentCellDefault.Item);
        }
    }

    public void UpdateCell(EquipmentType type, Item item)
    {
        EquipmentCell equipmentCell = FindCell(type);
        equipmentCell.AddItem(item);
        ChangedCell?.Invoke(equipmentCell);
    }

    public Item GetItem(EquipmentType type)
    {
        EquipmentCell equipmentCell = FindCell(type);
        return equipmentCell.Item;
    }

    private void Initialize()
    {
        EquipmentType[] types = (EquipmentType[])Enum.GetValues(typeof(EquipmentType));
        _equipmentCells = new EquipmentCell[types.Length];

        for (int i = 0; i < types.Length; i++) 
        {
            _equipmentCells[i] = new EquipmentCell(types[i], null);
        }
    }

    private EquipmentCell FindCell(EquipmentType type)
    {
        EquipmentCell equipmentCell = _equipmentCells.FirstOrDefault(equipmentCell => equipmentCell.Type == type);

        if (equipmentCell == null)
            throw new ArgumentNullException(nameof(equipmentCell));

        return equipmentCell;
    }
}