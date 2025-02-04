using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSettingAnimationLayer", menuName = "Animations/SettingAnimationLayer")]
public class SettingAnimationLayer : ScriptableObject
{
    [SerializeField] private TypeAnimationLayer _type;
    [SerializeField] private TypeAnimationLayer[] _addons;

    public TypeAnimationLayer Type => _type;
    public IReadOnlyList<TypeAnimationLayer> Addons => _addons;
}