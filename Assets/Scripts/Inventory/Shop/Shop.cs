using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopStorage _storage;
    [SerializeField] private DisplayerShopStorage _displayerShopStorage;
    [SerializeField] private DefaultSaleSlots _saleSlots;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _storage.Initialize(_saleSlots.Values);
        _displayerShopStorage.Initialize(_storage);
    }
}