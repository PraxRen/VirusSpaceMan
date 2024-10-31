using System;

public interface IStorageFighter
{
    event Action<IDamageable> ChangedDamageable;
    event Action<Weapon> ChangedWeapon;
}