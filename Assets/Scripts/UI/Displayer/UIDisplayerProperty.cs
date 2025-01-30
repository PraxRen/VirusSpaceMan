using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplayerProperty : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private Slider _slider;

    public void Initialize(string name, float value)
    {
        _textName.text = name;
        _slider.value = value;
    }
}