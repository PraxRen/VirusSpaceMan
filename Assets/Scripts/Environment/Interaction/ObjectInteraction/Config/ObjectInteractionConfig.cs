using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectInteractionConfig", menuName = "Interaction/ObjectInteraction/ObjectInteractionConfig")]
public class ObjectInteractionConfig : ScriptableObject
{
    [SerializeField] private bool _isLoop;
    [SerializeField] private int[] _animationInteractiveIndexes;

    public bool IsLoop => _isLoop;
    public IReadOnlyList<int> AnimationInteractiveIndexes => _animationInteractiveIndexes;
}