using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIShop : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IReadOnlyShop))] private MonoBehaviour _shopMonoBehaviour;
    [SerializeField] private DisplayerStorage _displayerStorage;
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
        ResetButtonsPay();
        _scrollButtonsPanel.SetActive(true);
    }

    private void OnDeactivated()
    {
        _shop.BeforeChangedActiveSlot -= HideSlot;
        _shop.ChangedActiveSlot -= ShowSlot;
        _scrollButtonsPanel.SetActive(false);

        foreach (UIButtonPay buttonPay in _buttonsPay)
            buttonPay.gameObject.SetActive(false);

        if (_shop.ActiveSlot != null)
            HideSlot(_shop.ActiveSlot);
    }

    private void OnInitialized(IReadOnlyTrader seller, IReadOnlyTrader buyer)
    {
        _displayerStorage.Initialize(seller.SimpleStorage);
    }

    private void ResetButtonsPay()
    {
        ISaleItem saleItem = _shop.ActiveSlot.GetItem() as ISaleItem;

        if (saleItem == null)
            throw new InvalidCastException(nameof(saleItem));    

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

    private void HideSlot(ISimpleSlot simpleSlot)
    {
        _displayerStorage.Hide(simpleSlot);
    }

    private void ShowSlot(ISimpleSlot simpleSlot)
    {
        _displayerStorage.Show(simpleSlot);
        ResetButtonsPay();
    }
}