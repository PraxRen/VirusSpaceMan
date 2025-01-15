using UnityEngine;

public class Hit
{
    public Hit(IWeaponReadOnly weapon, Attack attack, Vector3 point, bool isRageAttack)
    {
        Weapon = weapon;
        Attack = attack;
        Point = point;
        IsRageAttack = isRageAttack;
    }

    public IWeaponReadOnly Weapon { get; }
    public Attack Attack { get; }
    public Vector3 Point { get; }
    public bool IsRageAttack { get; }
    public float BaseDamage => Weapon.GetDamage() + Attack.Damage;
}