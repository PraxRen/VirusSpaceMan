using System;
using UnityEngine;

public interface IFighterReadOnly
{
    event Action<IWeaponReadOnly> ChangedWeapon;
    public event Action RemovedWeapon;

    public bool IsAttack { get; }
    public IWeaponReadOnly Weapon { get; }
    public LayerMask LayerMaskDamageable { get; }
}