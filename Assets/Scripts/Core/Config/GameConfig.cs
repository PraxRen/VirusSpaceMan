using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private ShopConfig _shopConfig;

    public ShopConfig ShopConfig => _shopConfig;
}