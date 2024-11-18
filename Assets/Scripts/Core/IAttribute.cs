using System;

public interface IAttribute
{
    public event Action ValueChanged;

    public float MaxValue { get; }
    public float Value { get; }
}
