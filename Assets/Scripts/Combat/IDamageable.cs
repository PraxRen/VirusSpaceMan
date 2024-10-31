public interface IDamageable
{
    bool CanTakeDamage(IWeaponReadOnly weapon);

    void TakeDamage(IWeaponReadOnly weapon, float damage);

    bool CanDie(IWeaponReadOnly weapon, float damage);
}
