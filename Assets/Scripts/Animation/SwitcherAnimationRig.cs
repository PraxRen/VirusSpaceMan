using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DataAnimationRig))]
public class SwitcherAnimationRig : MonoBehaviour
{
    private DataAnimationRig _data;
    private Dictionary<TypeAnimationRig, Coroutine> _hashCorutineJobs = new Dictionary<TypeAnimationRig, Coroutine>();
    
    public AnimationRigSetting DefaultAnimationSetting { get; private set; }    
    public AnimationRigSetting CurrentAnimationSetting { get; private set; }

    public bool IsNotWork => _hashCorutineJobs.Count == 0;

    private void Awake()
    {
        _data = GetComponent<DataAnimationRig>();
        DefaultAnimationSetting = _data.GetSetting(TypeAnimationRig.DefaultAim);
        CurrentAnimationSetting = DefaultAnimationSetting;
    }

    public void ApplyDefaultAnimationRig()
    {
        float valueDefault = 1f;
        SetAnimationRig(DefaultAnimationSetting.Type, valueDefault);
    }

    public void SetAnimationRig(TypeAnimationRig type, float targetValue = 1f)
    {
        if (CurrentAnimationSetting.Type == type)
            return;

        AnimationRigSetting newSetting = _data.GetSetting(type);
        SetNewAnimationRig(CurrentAnimationSetting, newSetting, targetValue);
        CurrentAnimationSetting = newSetting;
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

        setting.Rig.weight = targetValue;
        _hashCorutineJobs.Remove(setting.Type);
    }
}