using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShopConfig", menuName = "Inventory/Shop/ShopConfig")]
public class ShopConfig : ScriptableObject
{
    [SerializeField][SerializeInterface(typeof(ISaleItem))] ScriptableObject[] _saleItemsScriptableObject;

    private ISaleItem[] _saleItems;

    private void OnValidate()
    {
        if (_saleItemsScriptableObject == null && _saleItemsScriptableObject.Length == 0)
            return;

        _saleItems = _saleItemsScriptableObject.Cast<ISaleItem>().ToArray();
    }

    public IReadOnlyCollection<ISaleItem> SaleItems => _saleItems;
}