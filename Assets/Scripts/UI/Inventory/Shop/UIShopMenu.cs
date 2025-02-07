using System;
using System.Collections;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.UIElements;

public class UIShopMenu : MonoBehaviour, IMenu
{
    [SerializeField] private UICastomButton _buttonPause;
    [SerializeField] private UICastomButton _buttonReturn;
    [SerializeField] private UICastomButton _buttonShopWeapon;
    [SerializeField] private UICastomButton _buttonShopArmor;
    [SerializeField][SerializeInterface(typeof(IReadOnlyShop))] private MonoBehaviour _shopWeaponMonoBehaviour;
    [SerializeField] private float _timeWait;

    private IReadOnlyShop _shopWeapon;

    private void Awake()
    {
        _shopWeapon = (IReadOnlyShop)_shopWeaponMonoBehaviour;
    }

    private void OnEnable() => Activate();

    private void OnDisable() => Deactivate();

    public void Activate()
    {
        gameObject.SetActive(true);
        _buttonShopWeapon.ClickUpInBounds += ActivateWeaponShop;
        _buttonReturn.ClickUpInBounds += DeactivateWeaponShop;
    }

    public void Deactivate()
    {
        _buttonShopWeapon.ClickUpInBounds -= ActivateWeaponShop;
        _buttonReturn.ClickUpInBounds -= DeactivateWeaponShop;
        gameObject.SetActive(false);
    }

    private void ActivateWeaponShop()
    {
        Action actionBefore = () =>
        {
            _buttonShopWeapon.gameObject.SetActive(false);
            _buttonShopArmor.gameObject.SetActive(false);
            _buttonPause.gameObject.SetActive(false);
        };

        Action actionAfter = () =>
        {
            _buttonReturn.gameObject.SetActive(true);
            _shopWeapon.Activate();
        };

        StartCoroutine(RunTimer(actionBefore, actionAfter, _timeWait));
    }

    private void DeactivateWeaponShop()
    {
        Action actionBefore = () =>
        {
            _shopWeapon.Deactivate();
            _buttonReturn.gameObject.SetActive(false);
        };

        Action actionAfter = () =>
        {
            _buttonPause.gameObject.SetActive(true);
            _buttonShopWeapon.gameObject.SetActive(true);
            _buttonShopArmor.gameObject.SetActive(true);
        };

        StartCoroutine(RunTimer(actionBefore, actionAfter, _timeWait));
    }

    private IEnumerator RunTimer(Action actionBefore,  Action actionAfter, float time)
    {
        actionBefore?.Invoke();
        yield return new WaitForSeconds(time);
        actionAfter?.Invoke();
    }
}