using UnityEngine;
using System.Linq;

public class SwitcherRigHit : MonoBehaviour 
{ 
    [SerializeField] private RigHit[] _rigsHit;
    [SerializeField] private float _distanceRaction;

    public void ApplyHit(Vector3 forceDirection, Vector3 hitPoint)
    {
        bool isFindedRigHit = false;
        float sqrDistanceRaction = _distanceRaction * _distanceRaction;

        foreach (RigHit rigHitInHash in _rigsHit)
        {
            float sqrDistance = (rigHitInHash.transform.position - hitPoint).sqrMagnitude;

            if (sqrDistance < sqrDistanceRaction)
            {
                rigHitInHash.AddForce(forceDirection);
                isFindedRigHit = true;
            }
        }

        if (isFindedRigHit == false)
        {
            RigHit rigHit = _rigsHit.OrderBy(rigHit => (rigHit.transform.position - hitPoint).sqrMagnitude).FirstOrDefault();
            rigHit?.AddForce(forceDirection);
        }
    }
}