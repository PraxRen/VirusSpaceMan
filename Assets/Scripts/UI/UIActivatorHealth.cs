using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActivatorHealth : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IHealth))] private MonoBehaviour _healthMonoBehaviour;
    [SerializeField] private GameObject _uiHealthGameObject;
    [SerializeField] private bool _isDisableAfterDied;
    [SerializeField] private bool _isEnableAfterTakeDamage;

    private IHealth _health;

    private void Awake()
    {
        _health = (IHealth)_healthMonoBehaviour;

        if (_isEnableAfterTakeDamage)
            _uiHealthGameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (_isDisableAfterDied)
            _health.Died += OnDied;

        if (_isEnableAfterTakeDamage)
            _health.AfterTakeDamage += OnAfterTakeDamage;
    }

    private void OnDisable()
    {
        if (_isDisableAfterDied)
            _health.Died -= OnDied;

        if (_isEnableAfterTakeDamage)
            _health.AfterTakeDamage -= OnAfterTakeDamage;
    }

    private void OnDied()
    {
        _uiHealthGameObject.SetActive(false);
    }

    private void OnAfterTakeDamage(Hit hit, float damage)
    {
        _uiHealthGameObject.SetActive(true);
    }
}
