using System;

public interface IHealth
{
    event Action Died;

    bool IsDied { get; }
}