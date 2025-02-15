using System;
using UnityEngine;

public class SavingScene : MonoBehaviour, ISceneLoaderHandler, ISceneUnloaderHandler 
{
    public event Action<ISceneUnloaderHandler> Unloaded;
    public event Action<ISceneLoaderHandler> Loaded;

    public void HandleLoadScene()
    {
        SavingSystem.Load();
        Loaded?.Invoke(this);
    }

    public void HandleUnloadScene()
    {
        SavingSystem.Save();
        Unloaded?.Invoke(this);
    }
}
