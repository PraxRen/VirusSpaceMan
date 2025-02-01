using UnityEngine;

[System.Serializable]
public struct GameCurrency
{
    [SerializeField] private TypeGameCurrency _type;
    [SerializeField] private string _iconResourceName;
    [SerializeField] private Color _color;

    public GameCurrency(TypeGameCurrency type, string iconResourceName, Color color)
    {
        _type = type;
        _iconResourceName = iconResourceName;
        _color = color;
    }

    public TypeGameCurrency Type => _type;
    public string IconResourceName => _iconResourceName;
    public Sprite Icon => Resources.Load<Sprite>(_iconResourceName);
    public Color Color => _color;
}