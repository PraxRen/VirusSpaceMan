using System;

public interface IWeaponReadOnly : ISurface
{
    event Action StartedAttack;
    event Action<ICollidable> �ollided;
    event Action<IDamageable> Hited;

    string Id { get; }
    WeaponConfig Config { get; }
    Attack CurrentAttack { get; }
}