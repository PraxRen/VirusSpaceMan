using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingGameCurrencies
{
    [SerializeField] private List<Price> _prices;

    public SettingGameCurrencies()
    {
        _prices = new List<Price>();

        foreach (GameCurrency gameCurrency in GameSetting.ShopConfig.Currencies)
            _prices.Add(new Price(gameCurrency, 1f));
    }

    public IReadOnlyCollection<Price> Prices => _prices;
}