using System;
using UnityEngine;

public interface IWeaponReadOnly : ISurface
{
    event Action StartedAttack;
    event Action<ICollidable> Collided;
    event Action<IDamageable> Hited;

    string Id { get; }
    WeaponConfig Config { get; }
    Attack CurrentAttack { get; }
    Vector3 Position { get; }

    bool CanCollide(Collider targetCollider);
    float GetDamage();
}