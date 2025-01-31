using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplayerPrice : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    public void Initialize(GameCurrency gameCurrency)
    {
        _image.sprite = gameCurrency.Icon;
        _text.text = gameCurrency.Price.ToString();
    }
}