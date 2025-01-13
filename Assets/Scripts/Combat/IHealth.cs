using System;

public interface IHealth : IDamageable, IAttribute
{
    event Action Died;

    bool IsDied { get; }
}