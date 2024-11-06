using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMatrixAction", menuName = "Action/MatrixAction")]
public class MatrixAction : ScriptableObject, IMatrixActionReanOnly
{
    public bool[] MatrixBlock;
    public bool[] MatrixCancel;

    private int _countTypeAction;
    private int _length;

    private void OnValidate()
    {
        _countTypeAction = Enum.GetValues(typeof(TypeAction)).Length;
        _length = _countTypeAction * _countTypeAction;
        InitializeMatrix(ref MatrixBlock);
        InitializeMatrix(ref MatrixCancel);
    }

    public bool CanUnblock(IAction currentAction, IAction nextAction)
    {
        return HasChosen(MatrixBlock, GetTypeAction(currentAction), GetTypeAction(nextAction)) == false;
    }

    public bool CanCancel(IAction currentAction, IAction nextAction)
    {
        return HasChosen(MatrixCancel, GetTypeAction(currentAction), GetTypeAction(nextAction));
    }

    private bool HasChosen(bool[] matrix, TypeAction typeActionOne, TypeAction typeActionTwo)
    {
        int index = GetIndex(typeActionOne, typeActionTwo);
        return matrix[index];
    }

    private int GetIndex(TypeAction typeActionOne, TypeAction typeActionTwo)
    {
        return (int)typeActionOne * _countTypeAction + (int)typeActionTwo;
    }

    private TypeAction GetTypeAction(IAction action)
    {
        return action switch
        {
            Mover => TypeAction.Mover,
            Fighter => TypeAction.Fighter,
            Health => TypeAction.Health,
            _ => throw new InvalidCastException(nameof(action))
        };
    }

    private void InitializeMatrix(ref bool[] matrix)
    {
        if (matrix == null || matrix.Length != _length)
        {
            matrix = new bool[_length];
        }
    }
}