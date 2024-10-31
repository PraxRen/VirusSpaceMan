using System;
using UnityEngine;

public class StorageFighterDEBUG : MonoBehaviour, IStorageFighter
{
    [SerializeField] private Health _health;
    [SerializeField] private Weapon _weapon;

    public event Action<IDamageable> ChangedDamageable;
    public event Action<Weapon> ChangedWeapon;

    private void Start()
    {
        ChangedDamageable?.Invoke(_health);
        ChangedWeapon?.Invoke(_weapon);
    }
}