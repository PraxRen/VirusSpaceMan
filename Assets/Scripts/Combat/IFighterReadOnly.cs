using System;
using System.Collections.Generic;
using UnityEngine;

public interface IFighterReadOnly : IDamageable
{
    event Action<IWeaponReadOnly> ChangedWeapon;
    event Action<IWeaponReadOnly> ActivatedWeapon;
    event Action<IWeaponReadOnly> DeactivatedWeapon;
    event Action RemovedWeapon;

    bool IsAttack { get; }
    IWeaponReadOnly Weapon { get; }
    IReadOnlyTargetTracker LookTracker { get; }
    LayerMask LayerMaskDamageable { get; }
    LayerMask LayerMaskCollision { get; }
    public IReadOnlyCollection<Collider> IgnoreColliders { get; }
}