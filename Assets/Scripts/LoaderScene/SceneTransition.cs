using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField][ReadOnly] private List<MonoBehaviour> _loaderHandlersMonoBehaviour;
    [SerializeField][ReadOnly] private List<MonoBehaviour> _unloaderHandlersMonoBehaviour;

    private ISceneLoaderHandler[] _loaderHandlers;
    private ISceneUnloaderHandler[] _unloaderHandlers;
    private List<ISceneLoaderHandler> _compilatedLoaderHandlers = new List<ISceneLoaderHandler>();
    private List<ISceneUnloaderHandler> _compilatedUnloaderHandlers = new List<ISceneUnloaderHandler>();
    private Action<SceneTransition> _callback;

#if UNITY_EDITOR
    [ContextMenu("Initialize")]
    private void Initialize()
    {
        MonoBehaviour[] allComponents = FindObjectsOfType<MonoBehaviour>();

        foreach (MonoBehaviour component in allComponents)
        {
            if (component is ISceneLoaderHandler loaderHandlers)
                _loaderHandlersMonoBehaviour.Add(component);

            if (component is ISceneUnloaderHandler unloaderHandlers)
                _unloaderHandlersMonoBehaviour.Add(component);
        }
    }
#endif

    private void Awake()
    {
        _loaderHandlers = _loaderHandlersMonoBehaviour.Cast<ISceneLoaderHandler>().ToArray();
        _unloaderHandlers = _unloaderHandlersMonoBehaviour.Cast<ISceneUnloaderHandler>().ToArray();
    }

    private IEnumerator Start()
    {
        yield return new WaitForNextFrameUnit();
        Load();
    }

    public SceneTransition Callback(Action<SceneTransition> action)
    {
        _callback = action;
        return this;
    }

    public void Unload()
    {
        if (_unloaderHandlers.Length == 0)
            RunCallback();

        foreach (ISceneUnloaderHandler unloaderHandler in _unloaderHandlers)
        {
            unloaderHandler.Unloaded += OnUnloaded;
            unloaderHandler.HandleUnloadScene();
        }
    }

    private void Load()
    {
        if (_loaderHandlers.Length == 0)
            SceneLoader.Callback(this);

        foreach (ISceneLoaderHandler loaderHandler in _loaderHandlers)
        {
            loaderHandler.Loaded += OnLoaded;
            loaderHandler.HandleLoadScene();
        }
    }

    private void OnLoaded(ISceneLoaderHandler loaderHandler)
    {
        loaderHandler.Loaded -= OnLoaded;
        _compilatedLoaderHandlers.Add(loaderHandler);

        if (_compilatedLoaderHandlers.Count == _loaderHandlers.Length)
        {
            _compilatedLoaderHandlers.Clear();
            SceneLoader.Callback(this);
        }
    }

    private void OnUnloaded(ISceneUnloaderHandler unloaderHandler)
    {
        unloaderHandler.Unloaded -= OnUnloaded;
        _compilatedUnloaderHandlers.Add(unloaderHandler);

        if (_compilatedUnloaderHandlers.Count == _unloaderHandlers.Length)
        {
            _compilatedUnloaderHandlers.Clear();
            RunCallback();
        }
    }

    private void RunCallback()
    {
        _callback?.Invoke(this);
        _callback = null;
    }
}