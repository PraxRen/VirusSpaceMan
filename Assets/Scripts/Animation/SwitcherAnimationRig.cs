using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SwitcherAnimationRig : MonoBehaviour
{
    private Dictionary<TypeAnimationRig, Coroutine> _hashCorutineJobs = new Dictionary<TypeAnimationRig, Coroutine>();



    private IEnumerator UpdateRigWeight(Rig rig, float targetValue, float time)
    {
        float startValue = rig.weight;
        targetValue = Mathf.Clamp01(targetValue);
        float elapsedTime = 0f;
        float delta = Mathf.Abs(targetValue - startValue);
        float speedUpdate = delta / time;

        while (Mathf.Approximately(rig.weight, targetValue) == false)
        {
            rig.weight = Mathf.Lerp(startValue, targetValue, speedUpdate * elapsedTime / delta);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rig.weight = targetValue;
        //_jobUpdateRigWeight = null;
    }
}