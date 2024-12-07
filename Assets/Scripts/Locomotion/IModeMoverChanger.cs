using System;

public interface IModeMoverChanger
{
    event Action<IModeMoverProvider> AddedModeMover;
    event Action<IModeMoverProvider> RemovedModeMover;
}