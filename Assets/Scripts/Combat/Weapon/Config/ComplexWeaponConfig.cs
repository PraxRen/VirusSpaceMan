using System;
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
    [SerializeField] private SettingAnimationLayer _settingAnimationLayer;
    [Header("AnimationRigProvider")]
    [SerializeField] private TypeAnimationRig _typeAnimationRig;
    [Header("ModeMoverProvider")]
    [SerializeField] private ModeMover _modeMover;
    [Header("Sale")]
    [SerializeField] private SettingGameCurrencies _settingGameCurrencies;
    [Header("Graphics")]
    [SerializeField] private Graphics _prefabGraphics;
    [SerializeField] private Vector3 _offsetPosition;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private Vector3 _scale;

    public string IdWeapon => _idWeapon;
    public float Damage => _damage;
    public float Force => _force;
    public float DistanceAttack => _distanceAttack;
    public float CooldownAttack => _cooldownAttack;
    public float DistanceNoise => _distanceNoise;
    public float FactorNoise => _factorAuidioVolume;
    public SurfaceType SurfaceType => _surfaceType;
    public IReadOnlyList<Attack> Attacks => _attacks;
    public EquipmentType Type => _equipmentType;
    public SettingAnimationLayer SettingAnimationLayer => _settingAnimationLayer;
    public TypeAnimationRig TypeAnimationRig => _typeAnimationRig;
    public ModeMover ModeMover => _modeMover;
    public SettingGameCurrencies SettingGameCurrencies => _settingGameCurrencies;
    public Graphics PrefabGraphics => _prefabGraphics;
    public Vector3 OffsetPosition => _offsetPosition;
    public Vector3 StartRotation => _rotation;
    public Vector3 Scale => _scale;

#if UNITY_EDITOR
    [ContextMenu("Reset SettingGameCurrencies")]
    private void ResetSettingGameCurrencies()
    {
        _settingGameCurrencies = new SettingGameCurrencies();
    }
#endif

    private void OnValidate()
    {
        _damage = Mathf.Clamp(_damage, 0, GameSetting.CombatConfig.MaxValueDamage);
        _distanceAttack = Mathf.Clamp(_distanceAttack, 0, GameSetting.CombatConfig.MaxValueDistance);
        ValidateAddon();
    }

    protected virtual void ValidateAddon() { }
}