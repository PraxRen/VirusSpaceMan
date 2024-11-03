using UnityEngine;

[CreateAssetMenu(fileName = "NewComplexWeaponConfig", menuName = "Combat/ComplexWeaponConfig")]
public class ComplexWeaponConfig : WeaponConfig, IAnimationLayerProvider, IModeMoverProvider
{
    [SerializeField] private TypeAnimationLayer _typeAnimationLayer;
    [SerializeField] private ModeMover _modeMover;

    public TypeAnimationLayer TypeAnimationLayer => _typeAnimationLayer;
    public ModeMover ModeMover => _modeMover;
}