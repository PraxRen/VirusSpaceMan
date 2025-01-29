using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class DisplayerSaleSlot3D : DisplayerSlot3D<ISaleItem>
{
    [SerializeField] private Transform _pointGraphics;
    [SerializeField] private UIDisplayer _ui;

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
    }

    private void InitializeItem(IComplexWeaponConfig complexWeaponConfig)
    {
        Graphics graphics = Instantiate(complexWeaponConfig.PrefabGraphics, _pointGraphics);
        graphics.Transform.localPosition += complexWeaponConfig.OffsetPosition;
        graphics.Transform.localEulerAngles = complexWeaponConfig.StartRotation;
        graphics.Transform.localScale = complexWeaponConfig.Scale;
        InitializeItem((ISaleItem)complexWeaponConfig);
    }

    private void InitializeItem(ISaleItem item) 
    {
        _ui.SetName(item.Name);
        _ui.SetDescription(item.Description);
    }
}