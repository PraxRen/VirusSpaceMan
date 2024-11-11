using UnityEngine;

public interface IMover : IMoverReadOnly, IAction
{
    bool CanMove();
    void Move(Vector2 direction);
    void LookAtDirection(Vector2 direction);
}
