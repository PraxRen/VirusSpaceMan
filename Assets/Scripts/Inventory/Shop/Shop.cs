using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shop : MonoBehaviour, IReadOnlyShop
{
    public struct ShopActiveSlot
    {
        public ShopActiveSlot(ISimpleSlot value, bool hasInEquipment, bool hasInStorage)
        {
            Value = value;
            HasInEquipment = hasInEquipment;
            HasInStorage = hasInStorage;
        }

        public ISimpleSlot Value { get; }
        public bool HasInEquipment { get; }
        public bool HasInStorage { get; }
    }

    [SerializeField][SerializeInterface(typeof(IReadOnlyButton))] private MonoBehaviour _buttonScrollNextMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyButton))] private MonoBehaviour _buttonScrollPreviousMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyButton))] private MonoBehaviour _buttonPayOneMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyButton))] private MonoBehaviour _buttonPayTwoMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyButton))] private MonoBehaviour _buttonEquipMonoBehaviour;
    [SerializeField] private Seller _seller;
    [SerializeField] private Buyer _buyer;
    [SerializeField] private bool _isEndlessInventory;

    private IReadOnlyButton _buttonScrollNext;
    private IReadOnlyButton _buttonScrollPrevious;
    private IReadOnlyButton _buttonPayOne;
    private IReadOnlyButton _buttonPayTwo;
    private IReadOnlyButton _buttonEquip;
    private List<ISimpleSlot> _slots = new List<ISimpleSlot>();
    private int _indexActiveSlot = -1;
    private int _maxCountItemForPay;

    public event Action<ShopActiveSlot> BeforeChangedActiveSlot;
    public event Action<ShopActiveSlot> ChangedActiveSlot;
    public event Action<IReadOnlyTrader, IReadOnlyTrader> Initialized;
    public event Action Activated;
    public event Action Deactivated;
    public event Action Emptied;

    public ShopActiveSlot ActiveSlot { get; private set; }

    private void Awake()
    {
        _buttonScrollNext = (IReadOnlyButton)_buttonScrollNextMonoBehaviour;
        _buttonScrollPrevious = (IReadOnlyButton)_buttonScrollPreviousMonoBehaviour;
        _buttonPayOne = (IReadOnlyButton)_buttonPayOneMonoBehaviour;
        _buttonPayTwo = (IReadOnlyButton)_buttonPayTwoMonoBehaviour;
        _buttonEquip = (IReadOnlyButton)_buttonEquipMonoBehaviour;
        _maxCountItemForPay = _isEndlessInventory ? 0 : 1; 
    }

    private void OnEnable()
    {
        _seller.Changed += Initialize;
    }

    private void OnDisable()
    {
        _seller.Changed -= Initialize;
    }

    public void Activate()
    {
        if (_slots.Count == 0)
            return;

        UpdateActiveSlot(0);
        _buttonScrollNext.ClickUpInBounds += OnChangedScrollNextItem;
        _buttonScrollPrevious.ClickUpInBounds += OnChangedScrollPreviousItem;
        _buttonPayOne.ClickUpInBounds += MakeTransactionOne;
        _buttonPayTwo.ClickUpInBounds += MakeTransactionTwo;
        _buttonEquip.ClickDown += OnClickEquip;
        Activated?.Invoke();
    }

    public void Deactivate()
    {
        _buttonScrollNext.ClickUpInBounds -= OnChangedScrollNextItem;
        _buttonScrollPrevious.ClickUpInBounds -= OnChangedScrollPreviousItem;
        _buttonPayOne.ClickUpInBounds -= MakeTransactionOne;
        _buttonPayTwo.ClickUpInBounds -= MakeTransactionTwo;
        _buttonEquip.ClickUpInBounds -= OnClickEquip;
        Deactivated?.Invoke();
    }

    private void Initialize(ISimpleStorage storage)
    {
        _indexActiveSlot = -1;
        ActiveSlot = default;
        _slots = storage.GetSlots().Where(slot => slot.IsEmpty == false).ToList();
        Initialized?.Invoke(_seller, _buyer);

        if (_slots.Count == 0)
            Emptied?.Invoke();
    }

    private void OnChangedScrollNextItem()
    {
        if (_slots.Count == 0)
            return;

        int newIndex = _indexActiveSlot == _slots.Count - 1 ? 0 : _indexActiveSlot + 1;

        if (newIndex == _indexActiveSlot)
            return;

        UpdateActiveSlot(newIndex);
    }

    private void OnChangedScrollPreviousItem()
    {
        if (_slots.Count == 0)
            return;

        int newIndex = _indexActiveSlot == 0 ? _slots.Count - 1 : _indexActiveSlot - 1;

        if (newIndex == _indexActiveSlot)
            return;

        UpdateActiveSlot(newIndex);
    }

    private void UpdateActiveSlot(int newIndex)
    {
        if (_indexActiveSlot != -1)
            BeforeChangedActiveSlot?.Invoke(ActiveSlot);

        _indexActiveSlot = newIndex;
        IObjectItem item = _slots[_indexActiveSlot].GetItem();
        ActiveSlot = new ShopActiveSlot(_slots[_indexActiveSlot], _buyer.HasItemInEquipment(item.Id), _buyer.HasItemInStorage(item.Id));
        ChangedActiveSlot?.Invoke(ActiveSlot);
    }

    private void MakeTransactionOne() => MakeTransaction(GameSetting.ShopConfig.Currencies[0]);

    private void MakeTransactionTwo() => MakeTransaction(GameSetting.ShopConfig.Currencies[1]);

    private void MakeTransaction(GameCurrency gameCurrency)
    {
        ISimpleSlot slot = _slots[_indexActiveSlot];
        ISaleItem item = _seller.SellItem(slot, _maxCountItemForPay);
        _buyer.BuyItem(item, 1);
        int indexSlot = _indexActiveSlot;

        if (slot.IsEmpty)
        {
            int newIndex = _indexActiveSlot == _slots.Count - 1 ? 0 : _indexActiveSlot + 1;
            ISimpleSlot newSlot = _slots[newIndex];
            Initialize(_seller.SimpleStorage);

            if (_slots.Count > 0)
                indexSlot = _slots.IndexOf(newSlot);
        }

        UpdateActiveSlot(indexSlot);
        SavingSystem.Save();
    }

    private void OnClickEquip()
    {
        IEquipmentItem equipmentItem = _slots[_indexActiveSlot].GetItem() as IEquipmentItem;

        if (equipmentItem == null)
            throw new InvalidCastException(nameof(equipmentItem));

        _buyer.Equip(equipmentItem);
        UpdateActiveSlot(_indexActiveSlot);
        SavingSystem.Save();
    }
}