using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActivatorPanelSrollTarget : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Image _imageLeft;
    [SerializeField] private Image _imageRight;

    private void OnEnable()
    {
        _scanner.ChangedTargets += OnChangedTargets;
    }

    private void OnDisable()
    {
        _scanner.ChangedTargets -= OnChangedTargets;
    }

    private void OnChangedTargets(IReadOnlyCollection<Collider> colliders)
    {
        bool isEnable = colliders.Count > 1;
        _imageLeft.enabled = isEnable;
        _imageRight.enabled = isEnable;
    }
}