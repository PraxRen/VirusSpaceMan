using UnityEngine;

public interface IHitReaction
{
    void Handle(IWeaponReadOnly weapon, Vector3 hitPoint, float damage);
}