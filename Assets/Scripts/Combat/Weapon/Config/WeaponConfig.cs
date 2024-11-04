using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponConfig", menuName = "Combat/WeaponConfig")]
public class WeaponConfig : Item
{
    [SerializeField] private string _idWeapon;
    [Min(0f)][SerializeField] private float _damage;
    [Min(0f)][SerializeField] private float _distanceAttack;
    [Min(0f)][SerializeField] private float _cooldownAttack;

    public string IdWeapon => _idWeapon;
    public float Damage => _damage;
    public float DistanceAttack => _distanceAttack;
    public float CooldownAttack => _cooldownAttack;
}