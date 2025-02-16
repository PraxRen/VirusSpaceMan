using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Shop;

public class UIShop : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IReadOnlyShop))] private MonoBehaviour _shopMonoBehaviour;
    [SerializeField] private DisplayerStorage _displayerStorage;
    [SerializeField] private UICastomButton _buttonEquip;
    [SerializeField] private GameObject _scrollButtonsPanel;
    [SerializeField] private UIButtonPay[] _buttonsPay;

    private IReadOnlyShop _shop;

    private void Awake()
    {
        _shop = (IReadOnlyShop)_shopMonoBehaviour;

        for (int i = 0; i < GameSetting.ShopConfig.Currencies.Count; i++)
        {
            GameCurrency gameCurrency = GameSetting.ShopConfig.Currencies[i];
            _buttonsPay[i].Initialize(gameCurrency);
            _buttonsPay[i].gameObject.SetActive(false);
        }
    }

    private void OnEnable() 
    {
        _shop.Initialized += OnInitialized;
        _shop.Activated += OnActivated;
        _shop.Deactivated += OnDeactivated;
        _shop.Emptied += OnDeactivated;
    }

    private void OnDisable() 
    {
        _shop.Initialized -= OnInitialized;
        _shop.Activated -= OnActivated;
        _shop.Deactivated -= OnDeactivated;
        _shop.Emptied -= OnDeactivated;
    }

    private void OnActivated()
    {
        _shop.BeforeChangedActiveSlot += HideSlot;
        _shop.ChangedActiveSlot += ShowSlot;
        ShowSlot(_shop.ActiveSlot);
        UpdateButtonsPay();
        _scrollButtonsPanel.SetActive(true);
    }

    private void OnDeactivated()
    {
        _shop.BeforeChangedActiveSlot -= HideSlot;
        _shop.ChangedActiveSlot -= ShowSlot;
        _scrollButtonsPanel.SetActive(false);

        foreach (UIButtonPay buttonPay in _buttonsPay)
            buttonPay.gameObject.SetActive(false);

        if (_shop.ActiveSlot.Value != null)
            HideSlot(_shop.ActiveSlot);
    }

    private void OnInitialized(IReadOnlyTrader seller, IReadOnlyTrader buyer)
    {
        _displayerStorage.Initialize(seller.SimpleStorage);
    }

    private void UpdateButtonsPay()
    {
        ISaleItem saleItem = _shop.ActiveSlot.Value.GetItem() as ISaleItem;

        if (saleItem == null)
            throw new InvalidCastException(nameof(saleItem));

        if (_shop.ActiveSlot.HasInStorage || _shop.ActiveSlot.HasInEquipment)
        {
            foreach (UIButtonPay buttonPay in _buttonsPay)
                buttonPay.gameObject.SetActive(false);

            return;
        }

        IEnumerable<TypeGameCurrency> typesGameCurrency = saleItem.SettingGameCurrencies.Prices.Where(price => price.Value > 0)
                                                                                               .Select(price => price.GameCurrency.Type);
        foreach (UIButtonPay buttonPay in _buttonsPay)
        {
            if (typesGameCurrency.Contains(buttonPay.GameCurrency.Type))
                buttonPay.gameObject.SetActive(true);
            else
                buttonPay.gameObject.SetActive(false);
        }
    }

    private void UpdateButtonEquip(ShopActiveSlot activeSlot)
    {
        if (activeSlot.HasInStorage || activeSlot.HasInEquipment)
        {
            _buttonEquip.gameObject.SetActive(true);

            if (activeSlot.HasInEquipment)
                _buttonEquip.Deactivate();
            else
                _buttonEquip.Activate();

            return;
        }

        _buttonEquip.gameObject.SetActive(false);
    }

    private void HideSlot(ShopActiveSlot activeSlot)
    {
        _displayerStorage.Hide(activeSlot.Value);
    }

    private void ShowSlot(ShopActiveSlot activeSlot)
    {
        _displayerStorage.Show(activeSlot.Value);
        UpdateButtonEquip(activeSlot);
        UpdateButtonsPay();
    }
}