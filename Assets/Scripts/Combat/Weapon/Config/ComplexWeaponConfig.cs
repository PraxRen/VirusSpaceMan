using UnityEngine;

[CreateAssetMenu(fileName = "NewComplexWeaponConfig", menuName = "Combat/ComplexWeaponConfig")]
public class ComplexWeaponConfig : WeaponConfig, IAnimationLayerProvider, IAnimationRigProvider, IModeMoverProvider
{
    [SerializeField] private TypeAnimationLayer _typeAnimationLayer;
    [SerializeField] private TypeAnimationRig _typeAnimationRig;
    [SerializeField] private ModeMover _defaultModeMover;
    [SerializeField] private ModeMover _activeModeMover;

    public TypeAnimationLayer TypeAnimationLayer => _typeAnimationLayer;
    public TypeAnimationRig TypeAnimationRig => _typeAnimationRig;
    public ModeMover DefaultModeMover => _defaultModeMover;
    public ModeMover ActiveModeMover => _activeModeMover;
}