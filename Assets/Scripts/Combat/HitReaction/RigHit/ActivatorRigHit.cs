using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SwitcherRigHit))]
public class ActivatorRigHit : MonoBehaviour, IHitReaction, IAction
{
    [SerializeField] private ActionScheduler _actionScheduler;

    private Transform _transform;
    private SwitcherRigHit _switherRigHit;

    private void Awake()
    {
        _transform = transform;
        _switherRigHit = GetComponent<SwitcherRigHit>();
    }

    public bool CanHandleHit(Hit hit, float damage)
    {
        if (enabled == false) 
            return false;

        if (_actionScheduler.CanStartAction(this) == false)
            return false;

        return true;
    }

    public void Cancel() { }

    public void HandleHit(Hit hit, float damage)
    {
        Vector3 forceDirection = CalculateForceDirection(hit.Weapon, hit.Point);
#if UNITY_EDITOR
        Debug.DrawLine(hit.Point, hit.Point + forceDirection, Color.yellow, 2f);
#endif
        _switherRigHit.ApplyHit(forceDirection, hit.Point);
        _actionScheduler.StartAction(this);
    }

    private Vector3 CalculateForceDirection(IWeaponReadOnly weapon, Vector3 hitPoint)
    {
        hitPoint = Vector3.Lerp(hitPoint, weapon.Position, 0.5f);
        Vector3 direction = (hitPoint - new Vector3(_transform.position.x, hitPoint.y, _transform.position.z)).normalized * -1;
        direction.y = 0;
        return direction;
    }
}