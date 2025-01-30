using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct GameCurrency
{
    [SerializeField][ReadOnly] private TypeGameCurrency _type;
    [Range(1f, 1000000f)][SerializeField] private float _price;
    [SerializeField] private Sprite _icon;

    public GameCurrency(TypeGameCurrency type, float price, Sprite icon)
    {
        _type = type;
        _price = price <= 0 ? throw new ArgumentOutOfRangeException(nameof(price)) : price;
        _icon = icon;
    }

    public TypeGameCurrency Type => _type;  
    public float Price => _price;
    public Sprite Icon => _icon;
}