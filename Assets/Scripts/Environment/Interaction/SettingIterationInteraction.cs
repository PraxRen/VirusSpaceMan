using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingIterationInteraction
{
    [SerializeField] private int _id;
    [SerializeField] private TypeGraphics[] _startInteractGraphics;
    [SerializeField] private TypeGraphics[] _beforeInteractGraphics;
    [SerializeField] private TypeGraphics[] _interactedGraphics;
    [SerializeField] private TypeGraphics[] _afterInteractGraphics;
    [SerializeField] private TypeGraphics[] _stopInteractGraphics;

    public int Id => _id;
    public IReadOnlyCollection<TypeGraphics> StartInteractGraphics => _startInteractGraphics;
    public IReadOnlyCollection<TypeGraphics> BeforeInteractGraphics => _beforeInteractGraphics;
    public IReadOnlyCollection<TypeGraphics> InteractedGraphics => _interactedGraphics;
    public IReadOnlyCollection<TypeGraphics> AfterInteractGraphics => _afterInteractGraphics;
    public IReadOnlyCollection<TypeGraphics> StopInteractGraphics => _stopInteractGraphics;
}