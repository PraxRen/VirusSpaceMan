using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopInputReader _shopInputReader;
    [SerializeField] private ShopStorage _storage;
    [SerializeField] private DisplayerStorage _displayerStorage;
    [SerializeField] private DefaultSaleSlots _saleSlots;

    private void OnEnable()
    {
        _shopInputReader.ChangedScrollNextItem += OnChangedScrollNextItem;
        _shopInputReader.ChangedScrollPreviousItem += OnChangedScrollPreviousItem;
    }

    private void OnDisable()
    {
        _shopInputReader.ChangedScrollNextItem -= OnChangedScrollNextItem;
        _shopInputReader.ChangedScrollPreviousItem -= OnChangedScrollPreviousItem;

    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _storage.Initialize(_saleSlots.Values);
        _displayerStorage.Initialize(_storage);
    }

    private void OnChangedScrollNextItem()
    {
        _displayerStorage.Next();
    }

    private void OnChangedScrollPreviousItem()
    {
        _displayerStorage.Previous();
    }
}