using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SavingSystem
{
    public static void Save()
    {
        ISerializationStrategy strategy = GameSetting.SavingSystemConfig.Strategy;
        string fileName = GameSetting.SavingSystemConfig.FileName;
        Dictionary<string, object> state = strategy.LoadFile(fileName);
        CaptureState(state);
        strategy.SaveFile(state, fileName);
#if UNITY_EDITOR
        Debug.Log("Save");
#endif
    }

    public static void Load()
    {
        ISerializationStrategy strategy = GameSetting.SavingSystemConfig.Strategy;
        string fileName = GameSetting.SavingSystemConfig.FileName;
        Dictionary<string, object> state = strategy.LoadFile(fileName);
        RestoreState(state);
#if UNITY_EDITOR
        Debug.Log("Load");
#endif
    }

    public static void Delete()
    {
        ISerializationStrategy strategy = GameSetting.SavingSystemConfig.Strategy;
        string fileName = GameSetting.SavingSystemConfig.FileName;
        strategy.DeleteFile(fileName);
    }

    private static void CaptureState(Dictionary<string, object> state)
    {
        foreach (SaveableEntity saveable in GameObject.FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.CaptureState();
        }

        state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
    }

    private static void RestoreState(Dictionary<string, object> state)
    {
        foreach (SaveableEntity saveable in GameObject.FindObjectsOfType<SaveableEntity>())
        {
            string id = saveable.Id;

            if (state.ContainsKey(id))
                saveable.RestoreState(state[id]);
        }
    }
}