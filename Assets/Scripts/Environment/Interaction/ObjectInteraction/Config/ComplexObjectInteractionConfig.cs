using UnityEngine;

[CreateAssetMenu(fileName = "NewComplexObjectInteractionConfig", menuName = "Interaction/ObjectInteraction/ComplexObjectInteractionConfig")]
public class ComplexObjectInteractionConfig : ObjectInteractionConfig, IAnimationLayerProvider, IAnimationRigProvider
{
    [SerializeField] private SettingAnimationLayer _settingAnimationLayer;
    [SerializeField] private TypeAnimationRig _typeAnimationRig;

    public SettingAnimationLayer SettingAnimationLayer => _settingAnimationLayer;
    public TypeAnimationRig TypeAnimationRig => _typeAnimationRig;
}