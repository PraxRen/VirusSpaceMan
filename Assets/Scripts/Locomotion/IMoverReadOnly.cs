using System;

public interface IMoverReadOnly
{
    event Action StepTook;

    float Speed { get; }
}