using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShopStorageConfig", menuName = "Inventory/Shop/ShopStorageConfig")]
public class ShopStorageConfig : ScriptableObject
{
    [SerializeField][SerializeInterface(typeof(ISaleItem))] ScriptableObject[] _saleItemsScriptableObject;

    private ISaleItem[] _saleItems;

    private void OnValidate()
    {
        if (_saleItemsScriptableObject == null && _saleItemsScriptableObject.Length == 0)
            return;

        _saleItems = _saleItemsScriptableObject.Cast<ISaleItem>().ToArray();
    }

    public IReadOnlyList<ISaleItem> SaleItems => _saleItems;
}