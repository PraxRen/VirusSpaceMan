using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponConfig", menuName = "Combat/WeaponConfig")]
public class WeaponConfig : Item
{
    [SerializeField] private string _idWeapon;
    [SerializeField] private float _damage;
    [Min(0f)][SerializeField] private float _distanceAttack;
    [Min(0f)][SerializeField] private float _cooldownAttack;
    [SerializeField] private List<Attack> _attacks;

    public string IdWeapon => _idWeapon;
    public float Damage => _damage;
    public float DistanceAttack => _distanceAttack;
    public float CooldownAttack => _cooldownAttack;
    public IReadOnlyList<Attack> Attacks => _attacks;
}