using UnityEngine;

public interface IMover : IMoverReadOnly
{
    void Move(Vector2 direction);
}
