using System;
using UnityEngine;

public interface IFighterReadOnly
{
    event Action<IWeaponReadOnly> ChangedWeapon;
    event Action RemovedWeapon;

    bool IsAttack { get; }
    IWeaponReadOnly Weapon { get; }
    LayerMask LayerMaskDamageable { get; }

    bool CanHit(Transform transform);
}