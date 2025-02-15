using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private ShopConfig _shopConfig;
    [SerializeField] private CombatConfig _combatConfig;
    [SerializeField] private SavingSystemConfig _savingSystemConfig;

    public ShopConfig ShopConfig => _shopConfig;
    public CombatConfig CombatConfig => _combatConfig;
    public SavingSystemConfig SavingSystemConfig => _savingSystemConfig;
}