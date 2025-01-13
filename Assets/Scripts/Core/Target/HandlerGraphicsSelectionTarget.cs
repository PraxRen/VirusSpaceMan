using UnityEngine;

public class HandlerGraphicsSelectionTarget : MonoBehaviour, IHandlerSelectionTarget
{
    [SerializeField] private GameObject _selectionTarget;

    public void Select() 
    {
        _selectionTarget.SetActive(true);
    }

    public void Deselect() 
    {
        _selectionTarget.SetActive(false);
    }
}