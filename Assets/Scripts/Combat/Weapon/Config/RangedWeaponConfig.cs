using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedWeaponConfig", menuName = "Combat/RangedWeaponConfig")]
public class RangedWeaponConfig : WeaponConfig
{
    [SerializeField] private ProjectileConfig _projectileConfig;

    public ProjectileConfig ProjectileConfig => _projectileConfig;
}