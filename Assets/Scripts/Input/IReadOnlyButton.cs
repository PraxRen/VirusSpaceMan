using System;

public interface IReadOnlyButton
{
    event Action ClickDown;
    event Action ClickUp;
    event Action ClickUpInBounds;
    event Action Activated;
    event Action Deactivated;
}