using UnityEngine;

public interface IMover : IMoverReadOnly
{
    bool CanMove();
    void Move(Vector2 direction);
}
