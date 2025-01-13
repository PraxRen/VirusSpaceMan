using TMPro;
using UnityEngine;

public class AttributeTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField][SerializeInterface(typeof(IAttribute))] private MonoBehaviour _attributeMonoBehaviour;

    private IAttribute _attribute;

    private void Awake()
    {
        _attribute = (IAttribute)_attributeMonoBehaviour;
    }

    private void OnEnable()
    {
        _attribute.ValueChanged += OnValueChenged;
        OnValueChenged();
    }

    private void OnDisable()
    {
        _attribute.ValueChanged -= OnValueChenged;
    }

    private void OnValueChenged()
    {
        _text.text = string.Format("{0:f0}", _attribute.Value);
    }
}