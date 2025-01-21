using System;

public interface ISceneUnloaderHandler
{
    event Action<ISceneUnloaderHandler> Unloaded;

    void HandleUnloadScene();
}