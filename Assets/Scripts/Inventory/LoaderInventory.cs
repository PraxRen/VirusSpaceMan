using UnityEngine;

public class LoaderInventory : MonoBehaviour
{
    [SerializeField] private Equipment _equipment;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private DefaultEquipmentSlots _defaultEquipmentSlots;
    [SerializeField] private DefaultInventorySlots _defaultInventorySlots;

    private void Awake()
    {
        Initilize();
    }

    private void Initilize()
    {
        _equipment.Initialize(_defaultEquipmentSlots.Values);
        _inventory.Initialize(_defaultInventorySlots.Values);
    }
}
