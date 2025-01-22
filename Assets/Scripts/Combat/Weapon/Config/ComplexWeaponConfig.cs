using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewComplexWeaponConfig", menuName = "Combat/ComplexWeaponConfig")]
public class ComplexWeaponConfig : Item, IComplexWeaponConfig
{
    [Header("Weapon")]
    [SerializeField] private string _idWeapon;
    [SerializeField] private float _damage;
    [SerializeField] private float _force;
    [Min(0f)][SerializeField] private float _distanceAttack;
    [Min(0f)][SerializeField] private float _cooldownAttack;
    [Min(0f)][SerializeField] private float _distanceNoise;
    [Range(0f, 1f)][SerializeField] private float _factorAuidioVolume;
    [SerializeField] private SurfaceType _surfaceType;
    [SerializeField] private List<Attack> _attacks;
    [Header("Equipment")]
    [SerializeField] private EquipmentType _equipmentType;
    [Header("AnimationLayerProvider")]
    [SerializeField] private TypeAnimationLayer _typeAnimationLayer;
    [Header("AnimationRigProvider")]
    [SerializeField] private TypeAnimationRig _typeAnimationRig;
    [Header("ModeMoverProvider")]
    [SerializeField] private ModeMover _modeMover;
    [Header("Sale")]
    [SerializeField] private SettingGameCurrencies _settingGameCurrencies;

    public string IdWeapon => _idWeapon;
    public float Damage => _damage;
    public float Force => _force;
    public float DistanceAttack => _distanceAttack;
    public float CooldownAttack => _cooldownAttack;
    public float DistanceNoise => _distanceNoise;
    public float FactorAuidioVolume => _factorAuidioVolume;
    public SurfaceType SurfaceType => _surfaceType;
    public IReadOnlyList<Attack> Attacks => _attacks;
    public EquipmentType EquipmentType => _equipmentType;
    public TypeAnimationLayer TypeAnimationLayer => _typeAnimationLayer;
    public TypeAnimationRig TypeAnimationRig => _typeAnimationRig;
    public ModeMover ModeMover => _modeMover;
    public SettingGameCurrencies SettingGameCurrencies => _settingGameCurrencies;
}