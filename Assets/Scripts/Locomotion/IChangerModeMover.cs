using System;

public interface IChangerModeMover
{
    event Action<IModeMoverProvider> ChangedModeMover;
}