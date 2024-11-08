using UnityEngine;

public interface IDamageable : ISurface, ITarget
{
    bool CanTakeDamage(IWeaponReadOnly weapon);

    void TakeDamage(IWeaponReadOnly weapon, float damage);

    bool CanDie(IWeaponReadOnly weapon, float damage);
}
