using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIDisplayerItem : MonoBehaviour
{
    public enum PropertyName
    {
        Damage,
        Distance,
        Accuracy
    }

    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private TextMeshProUGUI _textDescription;
    [Header("UIDisplayerProperty")]
    [SerializeField] private UIDisplayerProperty _prefab;
    [SerializeField] private Transform _propertyContent;
    [SerializeField] private HorizontalOrVerticalLayoutGroup _layoutGroup;
    [SerializeField] private int _maxCountProperty = 4;
    [SerializeField] private float _spacingDefault = -0.4f;
    [SerializeField] private float _stepForSpacing = 0.1f;

    private int _countProperty;

    public void SetName(string name)
    {
        _textName.text = name ?? throw new ArgumentNullException(nameof(name));
    }

    public void SetDescription(string description)
    {
        _textDescription.text = description ?? throw new ArgumentNullException(nameof(description));
    }

    public void AddProperty(UIDisplayerItem.PropertyName name, float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        if (_countProperty == _maxCountProperty)
            throw new ArgumentOutOfRangeException(nameof(_countProperty));

        if (_propertyContent.gameObject.activeSelf == false)
            _propertyContent.gameObject.SetActive(true);

        _layoutGroup.spacing += _stepForSpacing;
        UIDisplayerProperty property = Instantiate(_prefab, _propertyContent);
        property.Initialize(name.ToString(), value);
        _countProperty++;
    }
}