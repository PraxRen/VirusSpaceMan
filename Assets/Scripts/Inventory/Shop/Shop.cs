using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shop : MonoBehaviour, IReadOnlyShop
{
    [SerializeField][SerializeInterface(typeof(IReadOnlyButton))] private MonoBehaviour _buttonScrollNextMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyButton))] private MonoBehaviour _buttonScrollPreviousMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyButton))] private MonoBehaviour _buttonPayOneMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyButton))] private MonoBehaviour _buttonPayTwoMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IReadOnlyButton))] private MonoBehaviour _buttonEquipMonoBehaviour;
    [SerializeField] private Seller _seller;
    [SerializeField] private Buyer _buyer;

    private IReadOnlyButton _buttonScrollNext;
    private IReadOnlyButton _buttonScrollPrevious;
    private IReadOnlyButton _buttonPayOne;
    private IReadOnlyButton _buttonPayTwo;
    private IReadOnlyButton _buttonEquip;
    private List<ISimpleSlot> _slots = new List<ISimpleSlot>();
    private int _indexActiveSlot = -1;

    public event Action<ISimpleSlot> BeforeChangedActiveSlot;
    public event Action<ISimpleSlot> ChangedActiveSlot;
    public event Action<IReadOnlyTrader, IReadOnlyTrader> Initialized;
    public event Action Activated;
    public event Action Deactivated;
    public event Action Emptied;

    public ISimpleSlot ActiveSlot => _indexActiveSlot == -1 ? null : _slots[_indexActiveSlot];

    private void Awake()
    {
        _buttonScrollNext = (IReadOnlyButton)_buttonScrollNextMonoBehaviour;
        _buttonScrollPrevious = (IReadOnlyButton)_buttonScrollPreviousMonoBehaviour;
        _buttonPayOne = (IReadOnlyButton)_buttonPayOneMonoBehaviour;
        _buttonPayTwo = (IReadOnlyButton)_buttonPayTwoMonoBehaviour;
        _buttonEquip = (IReadOnlyButton)_buttonEquipMonoBehaviour;
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

        _indexActiveSlot = 0;
        _buttonScrollNext.ClickUpInBounds += OnChangedScrollNextItem;
        _buttonScrollPrevious.ClickUpInBounds += OnChangedScrollPreviousItem;
        _buttonPayOne.ClickUpInBounds += MakeTransactionOne;
        _buttonPayTwo.ClickUpInBounds += MakeTransactionTwo;
        _buttonEquip.ClickUpInBounds += OnClickEquip;
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
        _slots = storage.GetSlots().Where(slot => slot.IsEmpty == false).ToList();
        Initialized?.Invoke(_seller, _buyer);
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
        if (_indexActiveSlot == newIndex) 
            return;

        if (_indexActiveSlot != -1)
            BeforeChangedActiveSlot?.Invoke(ActiveSlot);

        _indexActiveSlot = newIndex;
        ChangedActiveSlot?.Invoke(ActiveSlot);
    }

    private void MakeTransactionOne() => MakeTransaction(GameSetting.ShopConfig.Currencies[0]);

    private void MakeTransactionTwo() => MakeTransaction(GameSetting.ShopConfig.Currencies[1]);

    private void MakeTransaction(GameCurrency gameCurrency)
    {
        ISimpleSlot slot = _slots[_indexActiveSlot];
        ISaleItem item = _seller.SellItem(slot, 1);
        _buyer.BuyItem(item, 1);

        if (slot.IsEmpty)
        {
            int newIndex = _indexActiveSlot == _slots.Count - 1 ? 0 : _indexActiveSlot + 1;
            ISimpleSlot newSlot = _slots[newIndex];
            Initialize(_seller.SimpleStorage);

            if (_slots.Count == 0)
            {
                Emptied?.Invoke();
                return;
            }

            UpdateActiveSlot(_slots.IndexOf(newSlot));
        }

        SavingSystem.Save();
    }

    private void OnClickEquip()
    {
        IEquipmentItem equipmentItem = _slots[_indexActiveSlot].GetItem() as IEquipmentItem;

        if (equipmentItem == null)
            throw new InvalidCastException(nameof(equipmentItem));


    }
}