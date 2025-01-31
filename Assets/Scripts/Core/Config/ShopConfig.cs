using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopConfig
{
    [SerializeField] private GameCurrency[] _currencies;

    public IReadOnlyCollection<GameCurrency> Currencies => _currencies;
}