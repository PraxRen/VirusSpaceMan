using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMatrixAction", menuName = "Action/MatrixAction")]
public class MatrixAction : ScriptableObject, IMatrixActionReanOnly
{
    public bool[,] MatrixBlock;
    public bool[,] MatrixCancel;

    private void OnEnable()
    {
        Init(ref MatrixBlock);
        Init(ref MatrixCancel);
    }

    public bool CanUnblock(IAction currentAction, IAction nextAction)
    {
        return HasChosen(MatrixBlock, GetTypeAction(currentAction), GetTypeAction(nextAction));
    }

    public bool CanCancel(IAction currentAction, IAction nextAction)
    {
        return HasChosen(MatrixCancel, GetTypeAction(currentAction), GetTypeAction(nextAction));
    }

    private bool HasChosen(bool[,] matrix, TypeAction typeActionOne, TypeAction typeActionTwo)
    {
        return matrix[(int)typeActionOne, (int)typeActionTwo];
    }

    private void Init(ref bool[,] matrix)
    {
        int size = Enum.GetValues(typeof(TypeAction)).Length;

        if (matrix == null || matrix.GetLength(0) != size)
        {
            matrix = new bool[size, size];
        }
    }

    private TypeAction GetTypeAction(IAction action)
    {
        TypeAction result;

        switch (action)
        {
            case Mover:
                result = TypeAction.Mover;
                break;
            case Fighter:
                result = TypeAction.Fighter;
                break;
            case Health:
                result = TypeAction.Health;
                break;
            default:
                throw new InvalidCastException(nameof(action));
        }

        return result;
    }
}
