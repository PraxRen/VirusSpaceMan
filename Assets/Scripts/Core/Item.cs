using UnityEngine;

public abstract class Item : ScriptableObject, IObjectItem, ISerializationCallbackReceiver
{
    [SerializeField][ReadOnly] private string _id;
    [SerializeField][TextArea] private string _description;
    [Min(1)][SerializeField] private int _limit;

    public string Id => _id;
    public int Limit => _limit;
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