using UnityEngine;

[CreateAssetMenu(fileName = "NewComplexRangedWeaponConfig", menuName = "Combat/ComplexRangedWeaponConfig")]
public class ComplexRangedWeaponConfig : RangedWeaponConfig, IAnimationLayerProvider, IModeMoverProvider
{
    [SerializeField] private TypeAnimationLayer _typeAnimationLayer;
    [SerializeField] private ModeMover _modeMover;

    public TypeAnimationLayer TypeAnimationLayer => _typeAnimationLayer;
    public ModeMover ModeMover => _modeMover;
}