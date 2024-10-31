using UnityEngine;

public abstract class Item : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField][ReadOnly] private string _id;

    public string Id => _id;

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (string.IsNullOrWhiteSpace(_id))
            _id = System.Guid.NewGuid().ToString();
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize() { }
}