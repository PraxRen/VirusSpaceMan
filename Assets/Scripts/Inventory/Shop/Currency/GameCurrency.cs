using System;
using UnityEngine;

[System.Serializable]
public struct GameCurrency
{
    [SerializeField][ReadOnly] private TypeGameCurrency _type;
    [Range(1f, 1000000f)][SerializeField] private float _price;

    public GameCurrency(TypeGameCurrency type, float price)
    {
        _type = type;
        _price = price <= 0 ? throw new ArgumentOutOfRangeException(nameof(price)) : price;
    }

    public TypeGameCurrency Type => _type;  
    public float Price => _price;
}