using System;
using UnityEngine;

public abstract class StateConfig : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField][ReadOnly] private string _id;
    [SerializeField][Range(0f,10f)] private float _timeSecondsWaitUpdate;

    public string Id => _id;
    protected float TimeSecondsWaitUpdate => _timeSecondsWaitUpdate;

    public abstract State CreateState(AICharacter character);

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (string.IsNullOrWhiteSpace(_id))
            _id = Guid.NewGuid().ToString();
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize() { }
}