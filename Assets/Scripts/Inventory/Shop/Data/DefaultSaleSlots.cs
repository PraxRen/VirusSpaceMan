using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDefaultSaleSlots", menuName = "Inventory/Shop/DefaultSaleSlots")]
public class DefaultSaleSlots : ScriptableObject
{
    [SerializeField] private DataSaleSlot[] _values;

    public IReadOnlyCollection<DataSaleSlot> Values => _values;
}