using System;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMatrixAction", menuName = "Action/MatrixAction")]
public class MatrixAction : ScriptableObject, IMatrixActionReanOnly
{
    public bool[] MatrixBlock;
    public bool[] MatrixCancel;

    public int SizeMatrix { get; private set; }

    private void OnValidate()
    {
        if (SizeMatrix == 0)
            SizeMatrix = Enum.GetValues(typeof(TypeAction)).Length;

        if (MatrixBlock == null)
            MatrixBlock = new bool[SizeMatrix * SizeMatrix];

        if (MatrixCancel == null)
            MatrixCancel = new bool[SizeMatrix * SizeMatrix];
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
        return (int)typeActionOne * SizeMatrix + (int)typeActionTwo;
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
}