using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetterTargetTrackerMove : MonoBehaviour, IHitReaction
{
    [SerializeField] private TargetTracker _moveTargetTracker;

    public bool CanHandleHit(Hit hit, float damage) => true;

    public void HandleHit(Hit hit, float damage)
    {
        _moveTargetTracker.SetTarget(hit.Weapon.Fighter, Vector3.zero);
    }
}
