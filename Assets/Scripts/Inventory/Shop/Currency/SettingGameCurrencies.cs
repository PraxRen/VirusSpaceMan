using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingGameCurrencies
{
    [SerializeField] private List<GameCurrency> _currencies;

    public SettingGameCurrencies()
    {
        _currencies = new List<GameCurrency>();

        foreach (TypeGameCurrency typeGameCurrency in Enum.GetValues(typeof(TypeGameCurrency))) 
            _currencies.Add(new GameCurrency(typeGameCurrency, 1f, null));                
    }

    public IReadOnlyCollection<GameCurrency> Currencies => _currencies;
}