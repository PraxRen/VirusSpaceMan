using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwitcherAnimationLayer : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SettingAnimationLayer _defaultSetting;

    private Dictionary<int, Coroutine> _hashCorutineJobs = new Dictionary<int, Coroutine>();

    public SettingAnimationLayer DefaultSetting => _defaultSetting;
    public SettingAnimationLayer CurrentSetting { get; private set; }
    public bool IsNotWork => _hashCorutineJobs.Count == 0;

    private void Awake()
    {
        CurrentSetting = DefaultSetting;
    }

    public void ApplyDefaultSetting(float pathTime = 0f)
    {
        SetSetting(DefaultSetting, pathTime);
    }

    public void SetSetting(SettingAnimationLayer setting, float pathTime = 0f)
    {
        if (CurrentSetting == setting)
            return;

        SetNewSetting(CurrentSetting, setting, pathTime);
        CurrentSetting = setting;
    }

    public void SetAddonLayer(TypeAnimationLayer addonLayer, float pathTime = 0f)
    {
        if (CurrentSetting.Addons.Contains(addonLayer) == false)
            return;

        SetWeightLayer(addonLayer, 1f, pathTime);
    }

    public void RemoveAddonLayer(TypeAnimationLayer addonLayer, float pathTime = 0f)
    {
        if (CurrentSetting.Addons.Contains(addonLayer) == false)
            return;

        SetWeightLayer(addonLayer, 0f, pathTime);
    }

    public int GetIndexCurrentSetting()
    {
        string name = CurrentSetting.Type.ToString();
        int result = _animator.GetLayerIndex(name);
        return result;
    }

    private void SetNewSetting(SettingAnimationLayer oldSetting, SettingAnimationLayer newSetting, float pathTime = 0f)
    {
        SetWeightLayer(oldSetting.Type, 0, pathTime);

        foreach (TypeAnimationLayer layerAddon in oldSetting.Addons)
            SetWeightLayer(layerAddon, 0, pathTime);

        SetWeightLayer(newSetting.Type, 1, pathTime);
    }

    private void SetWeightLayer(TypeAnimationLayer animationLayer, float targetWeight, float pathTime = 0f)
    {
        if (animationLayer == TypeAnimationLayer.None)
            return;

        if (animationLayer == TypeAnimationLayer.Default)
            return;

        int indexLayer = _animator.GetLayerIndex(animationLayer.ToString());

        if (_hashCorutineJobs.ContainsKey(indexLayer))
        {
            StopCoroutine(_hashCorutineJobs[indexLayer]);
        }

        if (pathTime == 0f)
        {
            _animator.SetLayerWeight(indexLayer, targetWeight);
            return;
        }

        float currentWeight = _animator.GetLayerWeight(indexLayer);
        _hashCorutineJobs[indexLayer] = StartCoroutine(UpdateWeightLayer(indexLayer, currentWeight, targetWeight, pathTime));
    }

    private IEnumerator UpdateWeightLayer(int indexLayer, float currentWeight, float targetWeight, float pathTime)
    {
        float pathRunningTime = 0f;

        while (pathTime > pathRunningTime)
        {
            pathRunningTime += Time.deltaTime;
            _animator.SetLayerWeight(indexLayer, Mathf.Lerp(currentWeight, targetWeight, pathRunningTime / pathTime));
            yield return null;
        }

        _animator.SetLayerWeight(indexLayer, targetWeight);
        _hashCorutineJobs.Remove(indexLayer);
    }
}