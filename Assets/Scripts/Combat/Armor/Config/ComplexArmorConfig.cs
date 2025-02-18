using UnityEngine;

[CreateAssetMenu(fileName = "NewComplexArmorConfig", menuName = "Combat/ComplexArmorConfig")]
public class ComplexArmorConfig : Item, IComplexArmorConfig
{
    [Header("Armor")]
    [SerializeField] private string _idArmor;
    [Range(0f,1f)][SerializeField] private float _value;
    [Range(0f, 1f)][SerializeField] private float _factorNoise;
    [SerializeField] private SurfaceType _surfaceType;
    [Header("Equipment")]
    [SerializeField] private EquipmentType _equipmentType;
    [Header("ModeMoverProvider")]
    [SerializeField] private ModeMover _modeMover;
    [Header("Sale")]
    [SerializeField] private SettingGameCurrencies _settingGameCurrencies;

    public string IdArmor => _idArmor;
    public float Value => _value;
    public float FactorNoise => _factorNoise;
    public SurfaceType SurfaceType => _surfaceType;
    public EquipmentType Type => _equipmentType;
    public ModeMover ModeMover => _modeMover;
    public SettingGameCurrencies SettingGameCurrencies => _settingGameCurrencies;


#if UNITY_EDITOR
    [ContextMenu("Reset SettingGameCurrencies")]
    private void ResetSettingGameCurrencies()
    {
        _settingGameCurrencies = new SettingGameCurrencies();
    }
#endif

    private void OnValidate()
    {
        _value = Mathf.Clamp(_value, 0, GameSetting.CombatConfig.MaxValueArmor);
        ValidateAddon();
    }

    protected virtual void ValidateAddon() { }
}