using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplayerProperty : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private TextMeshProUGUI _textValue;
    [SerializeField] private Slider _slider;

    public void Initialize(string name, float value, float maxValue)
    {
        _textName.text = name;
        _textValue.text = $"({value.ToString(CultureInfo.InvariantCulture)})";
        _slider.maxValue = maxValue;
        _slider.value = value;
    }
}