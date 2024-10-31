public interface IMatrixActionReanOnly
{
    bool CanUnblock(IAction currentAction, IAction nextAction);

    bool CanCancel(IAction currentAction, IAction nextAction);
}
