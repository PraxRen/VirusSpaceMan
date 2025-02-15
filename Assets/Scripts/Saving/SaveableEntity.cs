using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class SaveableEntity : MonoBehaviour
{
    [Tooltip("The unique ID is automatically generated in a scene file if left empty. Do not set in a prefab unless you want all instances to be linked.")]
    [SerializeField] private string _id;

    private static Dictionary<string, SaveableEntity> _globalLookup = new Dictionary<string, SaveableEntity>();

    public string Id => _id;

#if UNITY_EDITOR
    private void Update()
    {
        if (Application.IsPlaying(gameObject)) 
            return;

        if (string.IsNullOrEmpty(gameObject.scene.path))
            return;

        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty property = serializedObject.FindProperty(nameof(_id));

        if (string.IsNullOrEmpty(property.stringValue) || IsUnique(property.stringValue) == false)
        {
            property.stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }

        _globalLookup[property.stringValue] = this;
    }
#endif

    public object CaptureState()
    {
        Dictionary<string, object> state = new Dictionary<string, object>();

        foreach (ISaveable saveable in GetComponents<ISaveable>())
        {
            state[saveable.GetType().ToString()] = saveable.CaptureState();
        }

        return state;
    }

    public void RestoreState(object state)
    {
        Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

        foreach (ISaveable saveable in GetComponents<ISaveable>())
        {
            string typeString = saveable.GetType().ToString();

            if (stateDict.ContainsKey(typeString))
            {
                saveable.RestoreState(stateDict[typeString]);
            }
        }
    }

    private bool IsUnique(string id)
    {
        if (_globalLookup.ContainsKey(id) == false) 
            return true;

        if (_globalLookup[id] == this) 
            return true;

        if (_globalLookup[id] == null)
        {
            _globalLookup.Remove(id);
            return true;
        }

        if (_globalLookup[id].Id != id)
        {
            _globalLookup.Remove(id);
            return true;
        }

        return false;
    }
}