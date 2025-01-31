using UnityEngine;

public static class GameSettings
{
    private static readonly GameConfig _config;

    static GameSettings()
    {
        _config = GameConfig.LoadFromJson();

        if (_config == null)
        {
            Debug.LogError("GameConfig не найден! Используется резервная конфигурация.");
            _config = ScriptableObject.CreateInstance<GameConfig>(); // Заглушка
        }
    }

    public static ShopConfig ShopConfig => _config.ShopConfig;
}