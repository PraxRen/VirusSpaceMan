using System;
using System.Drawing.Drawing2D;
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

        DrawMatrix(NameMatrixBlock, types, matrixAction.MatrixBlock);
        DrawMatrix(NameMatrixCancel, types, matrixAction.MatrixCancel);
    }

    private void DrawMatrix(string nameMatrix, TypeAction[] types, bool[,] matrix)
    {
        EditorGUILayout.LabelField(nameMatrix);

        // ������ ���������� ��������
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(50)); // ������ ������ � ����� ������� ����

        for (int j = 0; j < types.Length; j++)
        {
            EditorGUILayout.LabelField(types[j].ToString(), GUILayout.Width(50));
        }

        EditorGUILayout.EndHorizontal();

        // ������ ����� � �����������
        for (int i = 0; i < types.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(types[i].ToString(), GUILayout.Width(70)); // ��������� ������
            for (int j = 0; j < types.Length; j++)
            {
                matrix[i, j] = EditorGUILayout.Toggle(matrix[i, j], GUILayout.Width(50));
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.LabelField("");
    }
}
