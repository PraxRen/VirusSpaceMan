using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameConfig))]
public class GameConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameConfig config = (GameConfig)target;

        if (GUILayout.Button("Сохранить в JSON"))
        {
            config.SaveToJson();
        }
    }
}