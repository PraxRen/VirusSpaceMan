using UnityEngine;

public interface IDamageable : ISurface
{
    public Vector3 Position { get; }

    bool CanTakeDamage(IWeaponReadOnly weapon);

    void TakeDamage(IWeaponReadOnly weapon, float damage);

    bool CanDie(IWeaponReadOnly weapon, float damage);
}
