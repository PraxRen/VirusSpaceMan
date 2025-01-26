using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ddd", menuName = "kkk")]
public class DEBUGFactory : ScriptableObject, IDisplayerSlotFactory<IComplexWeaponConfig>
{
    public IDisplayerSlot<IComplexWeaponConfig> Create(IReadOnlySlot<IComplexWeaponConfig> slot, IReadOnlyDisplayerStorage<IComplexWeaponConfig> displayerStorage)
    {
        throw new System.NotImplementedException();
    }
}
