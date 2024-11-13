using System;
using UnityEngine;

public interface IDamageable : ISurface
{
    public Vector3 Position { get; }

    event Action<IWeaponReadOnly, float> BeforeTakeDamage;
    event Action<IWeaponReadOnly, float> AfterTakeDamage;

    bool CanTakeDamage(IWeaponReadOnly weapon);
    void TakeDamage(IWeaponReadOnly weapon, float damage);
    bool CanDie(IWeaponReadOnly weapon, float damage);
}