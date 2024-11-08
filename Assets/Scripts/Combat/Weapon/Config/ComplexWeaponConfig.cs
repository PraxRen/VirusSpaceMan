using UnityEngine;

[CreateAssetMenu(fileName = "NewComplexWeaponConfig", menuName = "Combat/ComplexWeaponConfig")]
public class ComplexWeaponConfig : WeaponConfig, IAnimationLayerProvider, IAnimationRigProvider, IModeMoverProvider
{
    [SerializeField] private TypeAnimationLayer _typeAnimationLayer;
    [SerializeField] private TypeAnimationRig _typeAnimationRig;
    [SerializeField] private ModeMover _modeMover;

    public TypeAnimationLayer TypeAnimationLayer => _typeAnimationLayer;
    public TypeAnimationRig TypeAnimationRig => _typeAnimationRig;
    public ModeMover ModeMover => _modeMover;
}