using UnityEngine;

public class DisplayerSaleSlot3D : DisplayerSlot3D<ISaleItem>
{
    [SerializeField] private Transform _pointGraphics;
    [SerializeField] private UIDisplayerItem _ui;

    public override void Hide()
    {

    }

    public override void Show()
    {

    }

    protected override void ClearItem()
    {

    }

    protected override void Initialize(ISaleItem item)
    {
        switch (item)
        {
            case IComplexRangedWeaponConfig complexRangedWeaponConfig :
                InitializeItem(complexRangedWeaponConfig);
                break;
            case IComplexWeaponConfig complexWeaponConfig :
                InitializeItem(complexWeaponConfig);
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
        _ui.SetName(item.Name);
        _ui.SetDescription(item.Description);
    }
}