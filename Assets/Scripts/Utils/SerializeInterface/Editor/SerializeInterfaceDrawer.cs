using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(SerializeInterfaceAttribute))]
public class SerializeInterfaceDrawer : PropertyDrawer
{
    private const string ErrorMessage = "SerializeInterfaceAttribute works only with UnityEngine.Object";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (IsValidField() == false)
        {
            EditorGUI.HelpBox(position, ErrorMessage, MessageType.Error);
            return;
        }

        Type requiredType = (attribute as SerializeInterfaceAttribute).Type;
        UpdateDropIcon(position, requiredType);
        UpdatePropertyValue(property, requiredType);
        property.objectReferenceValue =  EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(Object), true);
    }

    public bool IsValidField()
    {
        return typeof(Object).IsAssignableFrom(fieldInfo.FieldType) || typeof(IEnumerable<Object>).IsAssignableFrom(fieldInfo.FieldType);
    }

    private bool IsInvalidObject(Object @object, Type requiredType)
    {
        return requiredType.IsAssignableFrom(@object) == false;
    }

    private void UpdatePropertyValue(SerializedProperty property, Type requiredType)
    {
        if (property.objectReferenceValue == null)
            return;

        if (IsInvalidObject(property.objectReferenceValue, requiredType))
            property.objectReferenceValue = null;
    }

    private void UpdateDropIcon(Rect position, Type requiredType)
    {
        if (position.Contains(Event.current.mousePosition) == false)
            return;

        foreach (Object reference in DragAndDrop.objectReferences)
        {
            if (IsInvalidObject(reference, requiredType))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                return;
            }
        }
    }
}