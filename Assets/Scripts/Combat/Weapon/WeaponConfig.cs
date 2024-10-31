using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponConfig", menuName = "Combat/WeaponConfig")]
public class WeaponConfig : Item, IAnimationLayerProvider
{
    [SerializeField] private float _damage;
    [SerializeField] private float _distanceAttack;
    [SerializeField] private float _cooldownAttack;
    [SerializeField] private TypeAnimationLayer _typeAnimationLayer;

    public float Damage => _damage;
    public float DistanceAttack => _distanceAttack;
    public float CooldownAttack => _cooldownAttack;
    public TypeAnimationLayer TypeAnimationLayer => _typeAnimationLayer;
}