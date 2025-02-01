using System;
using UnityEngine;

public abstract class DisplayerSlot3D<T> : MonoBehaviour, IDisplayerSlot<T> where T : IObjectItem
{
    [SerializeField] private Transform _pointGraphics;
    [SerializeField] private UIDisplayerItem _ui;

    public event Action<IReadOnlyDisplayerSlot<T>> Selected;

    public IReadOnlySlot<T> Slot { get; private set; }
    protected T Item { get; private set; }

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

    public void InitializeSlot(IReadOnlySlot<T> slot)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        ClearSlot();
        Slot = slot;
        Slot.AddedItem += OnAddedItem;
        Slot.RemovedItem += OnRemovedItem;

        if (Slot.Item == null)
            return;

        ResetItem(Slot.Item);

    }

    private void ResetItem(T item)
    {
        switch (item)
        {
            case IComplexRangedWeaponConfig complexRangedWeaponConfig:
                InitializeItem(complexRangedWeaponConfig);
                break;
            case IComplexWeaponConfig complexWeaponConfig:
                InitializeItem(complexWeaponConfig);
                break;
            case ISaleItem saleItem:
                InitializeItem(saleItem);
                break;
            default:
                InitializeItem(item);
                break;
        }
    }

    private void InitializeItem(IComplexRangedWeaponConfig complexRangedWeaponConfig)
    {
        InitializeItem((IComplexWeaponConfig)complexRangedWeaponConfig);
        _ui.AddProperty(UIDisplayerItem.PropertyName.Accuracy, complexRangedWeaponConfig.Accuracy);
    }

    private void InitializeItem(IComplexWeaponConfig complexWeaponConfig)
    {
        InitializeItem((ISaleItem)complexWeaponConfig);
        Graphics graphics = Instantiate(complexWeaponConfig.PrefabGraphics, _pointGraphics);
        graphics.Transform.localPosition += complexWeaponConfig.OffsetPosition;
        graphics.Transform.localEulerAngles = complexWeaponConfig.StartRotation;
        graphics.Transform.localScale = complexWeaponConfig.Scale;
        _ui.AddProperty(UIDisplayerItem.PropertyName.Damage, complexWeaponConfig.Damage);
        _ui.AddProperty(UIDisplayerItem.PropertyName.Distance, complexWeaponConfig.Damage);
    }

    private void InitializeItem(ISaleItem item)
    {
        InitializeItem((IObjectItem)item);

        foreach (Price price in item.SettingGameCurrencies.Prices)
        {
            _ui.SetPrice(price);
        }
    }

    private void InitializeItem(IObjectItem item)
    {
        _ui.SetName(item.Name);
        _ui.SetDescription(item.Description);
    }

    public void Hide()
    {

    }

    public void Show()
    {

    }

    protected abstract void ClearItem();

    private void ClearSlot()
    {
        if (Slot == null)
            return;

        Slot.AddedItem -= OnAddedItem;
        Slot.RemovedItem -= OnRemovedItem;
    }

    private void OnAddedItem(T item, int count)
    {
        if (Item != null)
            return;

        ResetItem(item);
    }

    private void OnRemovedItem(T item, int count)
    {
        if (Item == null)
            return;

        if (item != null)
            return;

        ClearItem();
    }
}