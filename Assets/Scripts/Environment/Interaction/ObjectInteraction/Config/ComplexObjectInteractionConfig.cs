using UnityEngine;

[CreateAssetMenu(fileName = "NewComplexObjectInteractionConfig", menuName = "Interaction/ObjectInteraction/ComplexObjectInteractionConfig")]
public class ComplexObjectInteractionConfig : ObjectInteractionConfig, IAnimationLayerProvider, IAnimationRigProvider
{
    [SerializeField] private TypeAnimationLayer _typeAnimationLayer;
    [SerializeField] private TypeAnimationRig _typeAnimationRig;

    public TypeAnimationLayer TypeAnimationLayer => _typeAnimationLayer;
    public TypeAnimationRig TypeAnimationRig => _typeAnimationRig;
}