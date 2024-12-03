using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVerticalKeypad : MonoBehaviour, IObjectInteraction
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _radiusStartPoint;

    public ITarget StartPoint { get; private set; }


    private void Awake()
    {
        StartPoint = new TargetTransform(_startPoint, _radiusStartPoint);
    }

    public void InteractBefore()
    {
        Debug.Log("InteractBefore");
    }
    
    public void Interact()
    {
        Debug.Log("Interact");
    }

    public void InteractAfter()
    {
        Debug.Log("InteractAfter");
    }
}