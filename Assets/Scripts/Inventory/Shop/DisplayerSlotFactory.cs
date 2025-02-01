using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDisplayerSlotFactory", menuName = "Inventory/Shop/DisplayerSlotFactory")]
public class DisplayerSlotFactory : ScriptableObject, IDisplayerSlotFactory
{
    [SerializeField] private DisplayerSlot3D _prefab;

    public IDisplayerSlot Create(ISimpleSlot slot, IReadOnlyDisplayerStorage displayerStorage)
    {
        DisplayerStorage baseDisplayerStorage = displayerStorage as DisplayerStorage;

        if (baseDisplayerStorage == null)
            throw new ArgumentNullException(nameof(baseDisplayerStorage));

        DisplayerSlot3D displayerSaleSlot3D = Instantiate(_prefab, baseDisplayerStorage.Transform);
        displayerSaleSlot3D.InitializeSlot(slot);
        displayerSaleSlot3D.Hide();
        return displayerSaleSlot3D;
    }
}