using UnityEngine;
using UnityEngine.UI;

public class UIButtonPay : UICastomButton
{
    [SerializeField] private Image _image;

    public GameCurrency GameCurrency { get; private set; }

    public void Initialize(GameCurrency gameCurrency)
    {
        GameCurrency = gameCurrency;
        _image.sprite = gameCurrency.Icon;
        _image.color = gameCurrency.Color;

        if (_image.TryGetComponent(out UIButtonChangerImageColor buttonChangerImageColor))
        {
            float factorAlpha = 0.75f;
            buttonChangerImageColor.ResetColors(gameCurrency.Color, new Color(gameCurrency.Color.r, gameCurrency.Color.g, gameCurrency.Color.b, gameCurrency.Color.a * factorAlpha));
        }
    }
}
