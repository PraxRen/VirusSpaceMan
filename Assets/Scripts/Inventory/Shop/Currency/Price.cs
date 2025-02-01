using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Price
{
    [SerializeField] private GameCurrency _gameCurrency;
    [Range(0f, 1000000f)][SerializeField] private float _value;

    public Price(GameCurrency gameCurrency, float value)
    {
        _gameCurrency = gameCurrency;
        _value = value <= 0 ? throw new ArgumentOutOfRangeException(nameof(value)) : value;
    }

    public float Value => _value;
    public GameCurrency GameCurrency => _gameCurrency;
}