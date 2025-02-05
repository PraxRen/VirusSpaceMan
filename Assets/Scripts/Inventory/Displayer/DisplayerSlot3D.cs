using System;
using UnityEngine;

public class DisplayerSlot3D : MonoBehaviour, IDisplayerSlot
{
    [SerializeField] private Transform _pointGraphics;
    [SerializeField] private UIDisplayerItem _ui;

    private Graphics _graphics;

    public ISimpleSlot Slot { get; private set; }
    public IGraphicsItem Item { get; private set; }

    private void OnEnable()
    {
        if (Slot == null)
            return;

        Slot.AddedItem += OnAddedItem;
        Slot.RemovedItem += OnRemovedItem;
    }

    private void OnDisable()
    {
        ClearSlot();
    }

    public void InitializeSlot(ISimpleSlot slot)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        ClearSlot();
        Slot = slot;
        Slot.AddedItem += OnAddedItem;
        Slot.RemovedItem += OnRemovedItem;
        IGraphicsItem item = Slot.GetItem() as IGraphicsItem;

        if (item == null)
            return;

        ResetItem(item);
    }

    private void ResetItem(IGraphicsItem item)
    {
        if (item == null)
            return;

        switch (item)
        {
            case IComplexRangedWeaponConfig complexRangedWeaponConfig:
                InitializeItem(complexRangedWeaponConfig);
                break;
            case IComplexWeaponConfig complexWeaponConfig:
                InitializeItem(complexWeaponConfig);
                break;
            case IGraphicsSaleItem graphicsSaleItem:
                InitializeItem(graphicsSaleItem);
                break;
            default:
                InitializeItem(item);
                break;
        }
    }

    private void InitializeItem(IComplexRangedWeaponConfig complexRangedWeaponConfig)
    {
        InitializeItem((IComplexWeaponConfig)complexRangedWeaponConfig);
        _ui.AddProperty(UIDisplayerItem.PropertyName.Accuracy, complexRangedWeaponConfig.Accuracy * 100, GameSetting.CombatConfig.MaxValueAccuracy * 100);
    }

    private void InitializeItem(IComplexWeaponConfig complexWeaponConfig)
    {
        InitializeItem((IGraphicsSaleItem)complexWeaponConfig);
        _ui.AddProperty(UIDisplayerItem.PropertyName.Damage, complexWeaponConfig.Damage, GameSetting.CombatConfig.MaxValueDamage);
        _ui.AddProperty(UIDisplayerItem.PropertyName.Distance, complexWeaponConfig.DistanceAttack, GameSetting.CombatConfig.MaxValueDistance);
    }

    private void InitializeItem(IGraphicsSaleItem item)
    {
        InitializeItem((IGraphicsItem)item);
        _graphics.Transform.localPosition += item.OffsetPosition;
        _graphics.Transform.localEulerAngles = item.StartRotation;
        _graphics.Transform.localScale = item.Scale;

        foreach (Price price in item.SettingGameCurrencies.Prices)
        {
            _ui.SetPrice(price);
        }
    }

    private void InitializeItem(IGraphicsItem item)
    {
        _ui.SetName(item.Name);
        _ui.SetDescription(item.Description);
        _graphics = Instantiate(item.PrefabGraphics, _pointGraphics);
        Item = item;
    }

    public void Hide()
    {
        _graphics.Deactivate();
        _ui.gameObject.SetActive(false);
    }

    public void Show()
    {
        _graphics.Activate();
        _ui.gameObject.SetActive(true);
    }

    public void ClearItem()
    {

    }

    private void ClearSlot()
    {
        if (Slot == null)
            return;

        Slot.AddedItem -= OnAddedItem;
        Slot.RemovedItem -= OnRemovedItem;
    }

    private void OnAddedItem()
    {
        if (Item != null)
            return;

        ResetItem(Slot.GetItem() as IGraphicsItem);
    }

    private void OnRemovedItem()
    {
        if (Item == null)
            return;

        if (Slot.GetItem() != null)
            return;

        ClearItem();
    }
}