using System;
using System.Linq;
using UnityEngine;

public class Equipment : MonoBehaviour, IEquipmentReadOnly
{
    [SerializeField] private EquipmentCell[] _defaultCells;

    private EquipmentCell[] _equipmentCells;

    public event Action<IEquipmentCellReanOnly> ChangedCell;

    private void OnValidate()
    {
        if (_defaultCells == null)
            return;

        if (_equipmentCells == null)
            return;

        Start();
    }

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        foreach (EquipmentCell cell in _equipmentCells) 
        {
            if (cell.Item != null)
                UpdateCell(cell.Type, cell.Item);
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
            EquipmentCell equipmentCellDefault = _defaultCells.FirstOrDefault(cell => cell.Type == types[i]);
            Item item = equipmentCellDefault?.Item;
            _equipmentCells[i] = new EquipmentCell(types[i], item);
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