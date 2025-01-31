using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    private const string ConfigPath = "gameconfig.json";

    [SerializeField] private ShopConfig _shopConfig;

    public ShopConfig ShopConfig => _shopConfig;

    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, ConfigPath), json);
        Debug.Log("GameConfig save in JSON.");
    }

    public static GameConfig LoadFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, ConfigPath);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameConfig config = ScriptableObject.CreateInstance<GameConfig>();
            JsonUtility.FromJsonOverwrite(json, config);

            return config;
        }

        return null;
    }
}