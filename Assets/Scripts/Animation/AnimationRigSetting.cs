using UnityEngine;
using UnityEngine.Animations.Rigging;

[System.Serializable]
public class AnimationRigSetting
{
    [SerializeField] private TypeAnimationRig _type;
    [SerializeField] private float _timeUpdate;
    [SerializeField] private Rig _rig;

    public TypeAnimationRig Type => _type;
    public float TimeUpdate => _timeUpdate;
    public Rig Rig => _rig;
}