using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectInteractionConfig", menuName = "Interaction/ObjectInteraction/ObjectInteractionConfig")]
public class ObjectInteractionConfig : ScriptableObject
{
    [SerializeField] private ModeInteractive _mode;
    [Tooltip("Enable for ModeInteractive.Timer")][Min(0f)][SerializeField] private float _timeModeTimer;
    [Range(0.3f, 5f)][SerializeField] private float _animationLoopTimeout;
    [SerializeField] private int[] _animationInteractiveIndexes;

    public ModeInteractive Mode => _mode;
    public float TimeModeTimer => _timeModeTimer;
    public float AnimationLoopTimeout => _animationLoopTimeout;
    public IReadOnlyList<int> AnimationInteractiveIndexes => _animationInteractiveIndexes;
}