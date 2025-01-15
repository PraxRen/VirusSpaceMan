using UnityEngine;

public interface IHitReaction
{
    bool CanHandleHit(Hit hit, float damage);
    void HandleHit(Hit hit, float damage);
}