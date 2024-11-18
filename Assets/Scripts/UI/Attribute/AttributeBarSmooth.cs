using System.Collections;
using UnityEngine;

public class AttributeBarSmooth : AttributeBar
{
    [SerializeField] private float _speedUpdateValueSlider;

    private Coroutine _jobUpdateValueSlider;

    protected override void OnValueChanged()
    {
        if (_jobUpdateValueSlider != null)
            StopCoroutine(_jobUpdateValueSlider);

        _jobUpdateValueSlider = StartCoroutine(UpdateValueSlider());
    }

    private IEnumerator UpdateValueSlider()
    {
        float targetValue = Attribute.Value / Attribute.MaxValue;

        while(Mathf.Approximately(Slider.value, targetValue) == false)
        {
            Slider.value = Mathf.MoveTowards(Slider.value, targetValue, _speedUpdateValueSlider * Time.deltaTime);
            yield return null;
        }

        Slider.value = targetValue;
       _jobUpdateValueSlider = null;
    }
}