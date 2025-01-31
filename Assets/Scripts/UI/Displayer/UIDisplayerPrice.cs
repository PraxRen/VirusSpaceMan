using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplayerPrice : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    public void Initialize(Price price)
    {
        _image.sprite = price.GameCurrency.Icon;
        _image.color = price.GameCurrency.Color;
        _text.text = price.Value.ToString();
    }
}