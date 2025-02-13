using UnityEngine;

public class UIShopInputScroll : MonoBehaviour
{
    [SerializeField] private ShopInputReader _shopInputReader;
    [SerializeField] private UICastomButton _buttonNext;
    [SerializeField] private UICastomButton _buttonPrevious;

    private void OnEnable()
    {
        _shopInputReader.BeforeScrollNextItem += OnBeforeScrollNextItem;
        _shopInputReader.ScrollNextItem += OnScrollNextItem;
        _shopInputReader.BeforeScrollPreviousItem += OnBeforeScrollPreviousItem;
        _shopInputReader.ScrollPreviousItem += OnScrollPreviousItem;
    }

    private void OnDisable()
    {
        _shopInputReader.BeforeScrollNextItem -= OnBeforeScrollNextItem;
        _shopInputReader.ScrollNextItem -= OnScrollNextItem;
        _shopInputReader.BeforeScrollPreviousItem -= OnBeforeScrollPreviousItem;
        _shopInputReader.ScrollPreviousItem -= OnScrollPreviousItem;
    }

    private void OnBeforeScrollNextItem()
    {
        _buttonNext.Down();
    }

    private void OnScrollNextItem()
    {
        _buttonNext.Up(true);
    }

    private void OnBeforeScrollPreviousItem()
    {
        _buttonPrevious.Down();   
    }

    private void OnScrollPreviousItem()
    {
        _buttonPrevious.Up(true);
    }
}