using UnityEngine;

public abstract class Item : ScriptableObject, IObjectItem, ISerializationCallbackReceiver
{
    [SerializeField][ReadOnly] private string _id;
    [SerializeField] private string _name;
    [SerializeField][TextArea] private string _description;
    [Range(1, 99)][SerializeField] private int _limitInSlot;
    [Range(0, 99)][SerializeField] private int _limitInAllStorage;

    public string Id => _id;
    public string Name => _name;
    public int LimitInSlot => _limitInSlot;
    public int LimitInAllStorage => throw new System.NotImplementedException();
    public string Description => _description;

#if UNITY_EDITOR
    [ContextMenu("Reset ID")]
    private void ClearId()
    {
        _id = null;
        ((ISerializationCallbackReceiver)this).OnAfterDeserialize();
    }
#endif

    public override int GetHashCode() => _id.GetHashCode();

    public override bool Equals(object obj) => Equals(obj as Item);

    public bool Equals(Item item) => Equals(item as IObjectItem);

    public bool Equals(IObjectItem item)
    {
        if (item == null)
            return false;

        return _id == item.Id;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (string.IsNullOrWhiteSpace(_id))
            _id = System.Guid.NewGuid().ToString();
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize() { }
}