using UnityEngine;

[System.Serializable]
public struct GameCurrency
{
    [SerializeField] private TypeGameCurrency _type;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Color _color;

    public GameCurrency(TypeGameCurrency type, Sprite icon, Color color)
    {
        _type = type;
        _icon = icon;
        _color = color;
    }

    public TypeGameCurrency Type => _type;
    public Sprite Icon => _icon;
    public Color Color => _color;
}