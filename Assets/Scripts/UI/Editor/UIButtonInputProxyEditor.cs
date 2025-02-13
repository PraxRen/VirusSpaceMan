#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.InputSystem.Utilities;

[CustomEditor(typeof(UIButtonInputProxy))]
public class UIButtonInputProxyEditor : Editor
{
    private const string NameFieldActionAsset = "_inputActionAsset";
    private const string NameFieldActionMapName = "_actionMapName";
    private const string NameFieldActionName = "_actionName";
    private const string NameFieldButton = "_targetButton";
    private const string NameSelectionFieldActionMap = "Action Map";
    private const string NameSelectionFieldAction = "Action";

    private SerializedProperty _inputActionAssetProp;
    private SerializedProperty _actionMapNameProp;
    private SerializedProperty _actionNameProp;
    private SerializedProperty _targetButtonProp;

    private void Awake()
    {
        _inputActionAssetProp = serializedObject.FindProperty(NameFieldActionAsset);
        _actionMapNameProp = serializedObject.FindProperty(NameFieldActionMapName);
        _actionNameProp = serializedObject.FindProperty(NameFieldActionName);
        _targetButtonProp = serializedObject.FindProperty(NameFieldButton);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_inputActionAssetProp);
        EditorGUILayout.PropertyField(_targetButtonProp);
        InputActionAsset asset = _inputActionAssetProp.objectReferenceValue as InputActionAsset;

        if (asset != null)
        {
            ReadOnlyArray<InputActionMap> actionMaps = asset.actionMaps;
            SetActionMapNameProp(actionMaps);
            SetActionNameProp(actionMaps);
        }
        else
        {
            EditorGUILayout.HelpBox("Assign InputActionAsset to select Action Map and Action.", MessageType.Warning);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void SetActionMapNameProp(ReadOnlyArray<InputActionMap> actionMaps)
    {
        string[] mapNames = actionMaps.Select(am => am.name).ToArray();
        int selectedMapIndex = GetSelectIndex(mapNames, _actionMapNameProp);
        selectedMapIndex = EditorGUILayout.Popup(NameSelectionFieldActionMap, selectedMapIndex, mapNames);
        _actionMapNameProp.stringValue = mapNames[selectedMapIndex];
    }

    private void SetActionNameProp(ReadOnlyArray<InputActionMap> actionMaps)
    {
        InputActionMap selectedMap = actionMaps.FirstOrDefault(am => am.name == _actionMapNameProp.stringValue);

        if (selectedMap == null)
            return;

        string[] actionNames = selectedMap.actions.Select(a => a.name).ToArray();
        int selectedActionIndex = GetSelectIndex(actionNames, _actionNameProp);
        selectedActionIndex = EditorGUILayout.Popup(NameSelectionFieldAction, selectedActionIndex, actionNames);
        _actionNameProp.stringValue = actionNames[selectedActionIndex];
    }

    private int GetSelectIndex(string[] names, SerializedProperty serializedProperty)
    {
        int index = 0;

        if (string.IsNullOrEmpty(serializedProperty.stringValue) == false)
        {
            index = System.Array.IndexOf(names, serializedProperty.stringValue);
            index = index < 0 ? 0 : index;
        }

        return index;
    }
}
#endif