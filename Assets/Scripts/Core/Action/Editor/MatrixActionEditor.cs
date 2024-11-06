using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MatrixAction))]
public class MatrixActionEditor : Editor
{
    private const string NameMatrixBlock = "Block Matrix";
    private const string NameMatrixCancel = "Cancel Matrix";

    public override void OnInspectorGUI()
    {
        MatrixAction matrixAction = (MatrixAction)target;
        TypeAction[] types = (TypeAction[])Enum.GetValues(typeof(TypeAction));
        int size = types.Length;
        DrawMatrix(NameMatrixBlock, types, matrixAction.MatrixBlock, size);
        DrawMatrix(NameMatrixCancel, types, matrixAction.MatrixCancel, size);
    }

    private void DrawMatrix(string nameMatrix, TypeAction[] types, bool[] matrix, int size)
    {
        EditorGUILayout.LabelField(nameMatrix);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(50));

        for (int j = 0; j < size; j++)
        {
            EditorGUILayout.LabelField(types[j].ToString(), GUILayout.Width(50));
        }

        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < size; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(types[i].ToString(), GUILayout.Width(70));

            for (int j = 0; j < size; j++)
            {
                int index = i * size + j;
                matrix[index] = EditorGUILayout.Toggle(matrix[index], GUILayout.Width(50));
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.LabelField("");
    }
}