using System;
using UnityEngine;

public abstract class Item : ScriptableObject, IEquatable<Item>, ISerializationCallbackReceiver
{
    [SerializeField][ReadOnly] private string _id;
    [SerializeField][TextArea] private string _description;

    public string Id => _id;
    public string Description => _description;

    public static bool operator ==(Item itemOne, Item itemTwo)
    {
        if (itemOne is null)
        {
            if (itemTwo is null)
                return true;

            return false;
        }

        return itemOne.Equals(itemTwo);
    }

    public static bool operator !=(Item itemOne, Item itemTwo) => !(itemOne == itemTwo);

    public override int GetHashCode() => _id.GetHashCode();

    public override bool Equals(object obj) => Equals(obj as Item);

    public bool Equals(Item item)
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