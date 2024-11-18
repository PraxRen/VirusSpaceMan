using UnityEngine;

public interface IHitReaction
{
    bool CanHandleHit(IWeaponReadOnly weapon, Vector3 hitPoint, float damage);
    void HandleHit(IWeaponReadOnly weapon, Vector3 hitPoint, float damage);
}