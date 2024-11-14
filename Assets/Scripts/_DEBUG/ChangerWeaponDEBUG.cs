using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerWeaponDEBUG : MonoBehaviour
{
    [SerializeField] private WeaponConfig _weaponConfig;
    [SerializeField] private Equipment _equipment;

    private void Update()
    {
        _equipment.UpdateCell(EquipmentType.Weapon, _weaponConfig);
    }
}