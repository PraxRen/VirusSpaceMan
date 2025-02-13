using UnityEngine;

public class LoaderShop : MonoBehaviour
{
    [SerializeField] private SaleItemStorage _storage;
    [SerializeField] private DefaultSaleSlots _saleSlots;

    private void Awake()
    {
        Initilize();
    }

    private void Initilize()
    {
        _storage.Initialize(_saleSlots.Values);
    }
}