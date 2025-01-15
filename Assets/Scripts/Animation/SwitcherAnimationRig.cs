using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DataAnimationRig))]
public class SwitcherAnimationRig : MonoBehaviour
{
    private DataAnimationRig _data;
    private Dictionary<TypeAnimationRig, Coroutine> _hashCorutineJobs = new Dictionary<TypeAnimationRig, Coroutine>();
    private AnimationRigSetting _defaultAnimationSetting;    
    private AnimationRigSetting _currentAnimationSetting;

    public TypeAnimationRig DefaultTypeAnimationRig => _defaultAnimationSetting.Type;
    public TypeAnimationRig CurrentTypeAnimationRig => _currentAnimationSetting.Type;
    public bool IsNotWork => _hashCorutineJobs.Count == 0;

    private void Awake()
    {
        _data = GetComponent<DataAnimationRig>();
        _defaultAnimationSetting = _data.GetSetting(TypeAnimationRig.DefaultAim);
        _currentAnimationSetting = _defaultAnimationSetting;
    }

    public void ApplyDefaultAnimationRig()
    {
        float valueDefault = 1f;
        SetAnimationRig(_defaultAnimationSetting.Type, valueDefault);
    }

    public void SetAnimationRig(TypeAnimationRig type, float targetValue = 1f)
    {
        if (_currentAnimationSetting.Type == type)
            return;

        AnimationRigSetting newSetting = _data.GetSetting(type);
        SetNewAnimationRig(_currentAnimationSetting, newSetting, targetValue);
        _currentAnimationSetting = newSetting;
    }

    private void SetNewAnimationRig(AnimationRigSetting oldAnimationRig, AnimationRigSetting newAnimationRig, float targetValue)
    {
        SetWeightRig(oldAnimationRig, 0f);
        SetWeightRig(newAnimationRig, targetValue);
    }

    private void SetWeightRig(AnimationRigSetting setting, float targetValue)
    {
        if (setting == null)
            return;

        if (setting.Type == TypeAnimationRig.None)
            return;

        if (_hashCorutineJobs.ContainsKey(setting.Type))
        {
            StopCoroutine(_hashCorutineJobs[setting.Type]);
            _hashCorutineJobs.Remove(setting.Type);
        }

        if (setting.TimeUpdate == 0f)
        {
            setting.Rig.weight = targetValue;
            return;
        }

        _hashCorutineJobs[setting.Type] = StartCoroutine(UpdateRigWeight(setting, targetValue));
    }

    private IEnumerator UpdateRigWeight(AnimationRigSetting setting, float targetValue)
    {
        float startValue = setting.Rig.weight;
        targetValue = Mathf.Clamp01(targetValue);
        float elapsedTime = 0f;
        float delta = Mathf.Abs(targetValue - startValue);
        float speedUpdate = delta / setting.TimeUpdate;

        while (Mathf.Approximately(setting.Rig.weight, targetValue) == false)
        {
            setting.Rig.weight = Mathf.Lerp(startValue, targetValue, speedUpdate * elapsedTime / delta);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return null; //иначе если условие выше Mathf.Approximately(setting.Rig.weight, targetValue) == false не сработает то _hashCorutineJobs[setting.Type] будет иметь значение null
        setting.Rig.weight = targetValue;
        _hashCorutineJobs.Remove(setting.Type);
    }
}