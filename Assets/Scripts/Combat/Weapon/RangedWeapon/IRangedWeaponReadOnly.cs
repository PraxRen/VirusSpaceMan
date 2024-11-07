using UnityEngine;

public interface IRangedWeaponReadOnly : IWeaponReadOnly
{
    Transform StartPoint { get; }
}