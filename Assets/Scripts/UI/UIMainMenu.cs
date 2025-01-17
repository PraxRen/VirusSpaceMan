using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour, IMenu, IChangerTime
{
    [SerializeField] private TimeScaler _timeScaler;

    private void OnEnable()
    {
        Activate();   
    }

    private void OnDisable()
    {
        Deactivate();
    }

    public void Activate()
    {
        _timeScaler.Pause(this);
        gameObject.SetActive(true);       
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        _timeScaler.Play(this);
    }
}