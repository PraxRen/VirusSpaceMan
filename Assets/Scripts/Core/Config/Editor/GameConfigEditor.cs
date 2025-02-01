using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameConfig))]
public class GameConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate GameSetting.cs"))
        {
            GameSettingGenerator.GenerateGameSetting();
        }
    }
}