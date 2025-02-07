using System;
using UnityEngine;

public class Shop : MonoBehaviour, IReadOnlyShop
{
    [SerializeField] private ShopInputReader _shopInputReader;
    [SerializeField] private ShopStorage _storage;
    [SerializeField] private DisplayerStorage _displayerStorage;
    [SerializeField] private DefaultSaleSlots _saleSlots;
    [SerializeField] private GameObject _buttonsPanel;

    public event Action Activated;  
    public event Action Deactivated;  

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

    public void Activate()
    {
        _displayerStorage.ShowActiveSlot();
        _buttonsPanel.SetActive(true);
        Activated?.Invoke();
    }

    public void Deactivate()
    {
        _displayerStorage.HideActiveSlot();
        _buttonsPanel.SetActive(false);
        Deactivated?.Invoke();
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