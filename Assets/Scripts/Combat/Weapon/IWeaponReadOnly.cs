using System;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponReadOnly : ISurface
{
    event Action StartedAttack;
    event Action<IDamageable, IWeaponReadOnly, Attack, Vector3> Hited;

    string Id { get; }
    WeaponConfig Config { get; }
    IFighterReadOnly Fighter { get; }
    Attack CurrentAttack { get; }
    Vector3 Position { get; }
    IReadOnlyCollection<Collider> Colliders { get; }

    bool CanCollide(Collider targetCollider);
    float GetDamage();
}