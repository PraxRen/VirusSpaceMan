using System;

public interface ISceneLoaderHandler
{
    event Action<ISceneLoaderHandler> Loaded;

    void HandleLoadScene();
}