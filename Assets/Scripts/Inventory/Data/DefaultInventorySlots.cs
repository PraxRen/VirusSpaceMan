using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDefaultInventorySlots", menuName = "Inventory/DefaultInventorySlots")]
public class DefaultInventorySlots : ScriptableObject
{
    [SerializeField] private DataSlot[] _values;

    public IReadOnlyCollection<DataSlot> Values => _values;
}