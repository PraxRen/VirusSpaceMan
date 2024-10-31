using System;

public interface IStepNotifier
{
    event Action CreatedStep;
}