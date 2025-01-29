using TMPro;
using UnityEngine;

public class UIDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private TextMeshProUGUI _textDescription;

    public void SetName(string name)
    {
        _textName.text = name;
    }

    public void SetDescription(string description)
    {
        _textDescription.text = description;
    }
}