using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedWeaponConfig", menuName = "Combat/RangedWeaponConfig")]
public class RangedWeaponConfig : WeaponConfig
{
    public static readonly float MaxRadiusAccuracy = 1.0f;

    [Range(0, 1)][SerializeField] private float _accuracy;
    [SerializeField] private ProjectileConfig _projectileConfig;

    public float Accuracy => _accuracy;
    public ProjectileConfig ProjectileConfig => _projectileConfig;
}