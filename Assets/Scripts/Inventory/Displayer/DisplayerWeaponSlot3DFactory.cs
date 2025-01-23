using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDisplayerWeaponSlot3DFactory", menuName = "Inventory/DisplayerWeaponSlot3DFactory")]
public class DisplayerWeaponSlot3DFactory : ScriptableObject, IDisplayerSlotFactory<IComplexWeaponConfig>
{
    [SerializeField] private DisplayerWeaponSlot3D _prefab;

    public IDisplayerSlot<IComplexWeaponConfig> Create(IReadOnlySlot<IComplexWeaponConfig> slot, IDisplayerStorage<IComplexWeaponConfig> displayerStorage)
    {
        DisplayerStorageMonoBehaviour<IComplexWeaponConfig> displayerStorageMonoBehaviour = displayerStorage as DisplayerStorageMonoBehaviour<IComplexWeaponConfig>;

        if (displayerStorageMonoBehaviour == null)
            throw new ArgumentNullException(nameof(displayerStorageMonoBehaviour));

        DisplayerWeaponSlot3D displayerWeaponSlot = Instantiate(_prefab, displayerStorageMonoBehaviour.Transform);
        displayerWeaponSlot.Initilize(slot);
        return displayerWeaponSlot;
    }
}