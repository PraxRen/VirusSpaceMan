using System;
using UnityEngine;

public interface IDamageable : ISurface
{
    public Vector3 Position { get; }

    event Action<IWeaponReadOnly, Vector3, float> BeforeTakeDamage;
    event Action<IWeaponReadOnly, Vector3, float> AfterTakeDamage;

    bool CanTakeDamage(IWeaponReadOnly weapon);
    void TakeDamage(IWeaponReadOnly weapon, Vector3 hitPoint, float damage);
    bool CanDie(IWeaponReadOnly weapon, float damage);
}