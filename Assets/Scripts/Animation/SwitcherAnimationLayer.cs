using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitcherAnimationLayer : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Dictionary<int, Coroutine> _hashCorutineJobs = new Dictionary<int, Coroutine>();

    public TypeAnimationLayer DefaultAnimationLayer { get; private set; } = TypeAnimationLayer.Default;
    public TypeAnimationLayer CurrentAnimationLayer { get; private set; } = TypeAnimationLayer.Default;
    public bool IsNotWork => _hashCorutineJobs.Count == 0;

    public void SetAnimationLayer(TypeAnimationLayer animationLayer, float pathTime = 0f)
    {
        if (CurrentAnimationLayer == animationLayer)
            return;

        SetNewAnimationLayer(CurrentAnimationLayer, animationLayer, pathTime);
        CurrentAnimationLayer = animationLayer;
    }

    public void ApplyDefaultAnimationLayer(float pathTime = 0f)
    {
        SetAnimationLayer(DefaultAnimationLayer, pathTime);
    }

    public int GetIndexCurrentMoverAnimationLayer()
    {
        string name = CurrentAnimationLayer.ToString();
        int result = _animator.GetLayerIndex(name);
        return result;
    }

    private void SetNewAnimationLayer(TypeAnimationLayer oldAnimationLayer, TypeAnimationLayer newAnimationLayer, float pathTime = 0f)
    {
        SetWeightLayer(oldAnimationLayer, 0, pathTime);
        SetWeightLayer(newAnimationLayer, 1, pathTime);
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