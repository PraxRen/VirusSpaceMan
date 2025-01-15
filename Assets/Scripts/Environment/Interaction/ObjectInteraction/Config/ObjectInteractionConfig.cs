using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectInteractionConfig", menuName = "Interaction/ObjectInteraction/ObjectInteractionConfig")]
public class ObjectInteractionConfig : ScriptableObject
{
    [SerializeField] private ModeInteractive _mode;
    [Tooltip("It only works in the ModeInteractive.Timer")][Min(0f)][SerializeField] private float _timeModeTimer;
    [Range(0.3f, 5f)][SerializeField] private float _iterationLoopTimeout;
    [SerializeField] private SettingIterationInteraction[] _settingIterations;

    public ModeInteractive Mode => _mode;
    public float TimeModeTimer => _timeModeTimer;
    public float AnimationLoopTimeout => _iterationLoopTimeout;
    public IReadOnlyList<SettingIterationInteraction> SettingIterations => _settingIterations;
}