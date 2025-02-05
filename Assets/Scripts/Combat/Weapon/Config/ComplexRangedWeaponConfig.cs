using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedWeaponConfig", menuName = "Combat/RangedWeaponConfig")]
public class ComplexRangedWeaponConfig : ComplexWeaponConfig, IComplexRangedWeaponConfig
{
    [Header("RangedWeapon")]
    [Range(0, 1)][SerializeField] private float _accuracy;
    [SerializeField] private ProjectileConfig _projectileConfig;

    public float Accuracy => _accuracy;
    public ProjectileConfig ProjectileConfig => _projectileConfig;

    protected override void ValidateAddon()
    {
        _accuracy = Mathf.Clamp(_accuracy, 0f, GameSetting.CombatConfig.MaxValueAccuracy);
    }
}