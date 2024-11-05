using System;

public interface IWeaponReadOnly : ISurface
{
    event Action StartedAttack;
    event Action<ICollidable> Ñollided;
    event Action<IDamageable> Hited;

    string Id { get; }
    WeaponConfig Config { get; }
    Attack CurrentAttack { get; }
}