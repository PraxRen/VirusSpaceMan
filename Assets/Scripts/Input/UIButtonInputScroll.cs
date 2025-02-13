using UnityEngine;

public class UIButtonInputScroll : MonoBehaviour
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
        //Debug.Log("BeforeScrollNext");
        _buttonNext.Down();
    }

    private void OnScrollNextItem()
    {
        //Debug.Log("ScrollNext");
        _buttonNext.Up(true);
    }

    private void OnBeforeScrollPreviousItem()
    {
        //Debug.Log("BeforeScrollPrevious");
        _buttonPrevious.Down();   
    }

    private void OnScrollPreviousItem()
    {
        //Debug.Log("ScrollPrevious");
        _buttonPrevious.Up(true);
    }
}