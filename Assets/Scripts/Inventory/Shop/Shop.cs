using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopInputReader _shopInputReader;
    [SerializeField] private ShopStorage _storage;
    [SerializeField] private DisplayerStorage _displayerStorage;
    [SerializeField] private DefaultSaleSlots _saleSlots;

    private void OnEnable()
    {
        _shopInputReader.ChangedScrollNextTarget += OnChangedScrollNextTarget;
        _shopInputReader.ChangedScrollPreviousTarget += OnChangedScrollPreviousTarget;
    }

    private void OnDisable()
    {
        _shopInputReader.ChangedScrollNextTarget -= OnChangedScrollNextTarget;
        _shopInputReader.ChangedScrollPreviousTarget -= OnChangedScrollPreviousTarget;

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

    private void OnChangedScrollNextTarget()
    {
        _displayerStorage.Next();
    }

    private void OnChangedScrollPreviousTarget()
    {
        _displayerStorage.Previous();
    }
}