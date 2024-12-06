using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectInteractionConfig", menuName = "Interaction/ObjectInteraction/ObjectInteractionConfig")]
public class ObjectInteractionConfig : ScriptableObject
{
    [SerializeField] private bool _isLoop;
    [Range(0.3f, 5f)][SerializeField] private float _animationLoopTimeout;
    [SerializeField] private int[] _animationInteractiveIndexes;

    public bool IsLoop => _isLoop;
    public float AnimationLoopTimeout => _animationLoopTimeout;
    public IReadOnlyList<int> AnimationInteractiveIndexes => _animationInteractiveIndexes;
}