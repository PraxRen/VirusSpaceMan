using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader 
{
    private static Action<SceneTransition> _callback;
    private static AsyncOperation _sceneAsyncOperation;

    public static float GetLoadProgress() => _sceneAsyncOperation != null ? _sceneAsyncOperation.progress : 1f;

    public static void Callback(SceneTransition sceneTransition) => _callback?.Invoke(sceneTransition);

    public static void Load(TypeScene scene) 
    {
        _callback = (callback) =>
        {
            _callback = (callbackTargetScene) => _callback = null;
            callback.StartCoroutine(LoadSceneAsync(scene));
        };

        Unload((callback) => callback.StartCoroutine(LoadSceneAsync(TypeScene.Load)));
    }

    private static void Unload(Action<SceneTransition> callback)
    {
        SceneTransition sceneTransition = FindSceneTransition();
        sceneTransition.Callback(callback).Unload();
    }

    private static SceneTransition FindSceneTransition()
    {
        SceneTransition sceneTransition = null;
        Scene activeScene = SceneManager.GetActiveScene();
        activeScene.GetRootGameObjects().FirstOrDefault(gameobject => gameobject.TryGetComponent(out sceneTransition));

        if (sceneTransition == null)
            throw new InvalidOperationException($"Сomponent {nameof(SceneTransition)} not found in scene {activeScene.name}");

        return sceneTransition;
    }

    private static IEnumerator LoadSceneAsync(TypeScene scene) 
    {

        _sceneAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while (!_sceneAsyncOperation.isDone) 
        {
            yield return null;
        }

        _sceneAsyncOperation = null;
    }
}