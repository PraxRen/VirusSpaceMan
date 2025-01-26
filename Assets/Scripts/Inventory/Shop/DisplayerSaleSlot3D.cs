using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayerSaleSlot3D : DisplayerSlot3D<ISaleItem>
{
    public override void Hide()
    {

    }

    public override void Show()
    {

    }

    protected override void ClearItem()
    {

    }

    protected override void InitializeItem(ISaleItem item)
    {
        switch (item)
        {
            case IComplexRangedWeaponConfig complexRangedWeaponConfig :
                break;
            case IComplexWeaponConfig complexWeaponConfig :
                break;
            default:
                InitializeSaleItem(item);
                break;
        }
    }

    private void InitializeSaleItem(ISaleItem item) 
    {
        
    }
}