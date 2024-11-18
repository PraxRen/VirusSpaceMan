using System.Linq;
using UnityEngine;

public class DataAnimationRig : MonoBehaviour
{
    [SerializeField] private AnimationRigSetting[] _settings;

    public AnimationRigSetting GetSetting(TypeAnimationRig type)
    {
        return _settings.FirstOrDefault(setting => setting.Type == type);
    }
}
