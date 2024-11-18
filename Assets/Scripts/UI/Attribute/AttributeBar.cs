using UnityEngine;
using UnityEngine.UI;

public class AttributeBar : MonoBehaviour
{
    [SerializeField] protected Slider Slider;
    [SerializeField][SerializeInterface(typeof(IAttribute))] private MonoBehaviour _attributeMonoBehaviour;

    protected IAttribute Attribute { get; private set; }

    private void Awake()
    {
        Attribute = (IAttribute)_attributeMonoBehaviour;
    }

    private void OnEnable()
    {
        Attribute.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        Attribute.ValueChanged -= OnValueChanged;
    }

    protected virtual void OnValueChanged()
    {
        Slider.value = Attribute.Value / Attribute.MaxValue;
    }
}