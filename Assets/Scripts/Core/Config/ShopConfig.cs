using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopConfig
{
    [SerializeField] private GameCurrency[] _currencies;

    public ShopConfig(GameCurrency[] currencies)
    {
        _currencies = currencies;
    }

    public IReadOnlyList<GameCurrency> Currencies => _currencies;
}