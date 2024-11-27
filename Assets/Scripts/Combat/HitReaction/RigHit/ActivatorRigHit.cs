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

    public bool CanHandleHit(IWeaponReadOnly weapon, Vector3 hitPoint, float damage)
    {
        if (enabled == false) 
            return false;

        if (_actionScheduler.CanStartAction(this) == false)
            return false;

        return true;
    }

    public void Cancel() { }

    public void HandleHit(IWeaponReadOnly weapon, Vector3 hitPoint, float damage)
    {
        Vector3 forceDirection = CalculateForceDirection(weapon, hitPoint);
#if UNITY_EDITOR
        Debug.DrawLine(hitPoint, hitPoint + forceDirection, Color.yellow, 2f);
#endif
        _switherRigHit.ApplyHit(forceDirection, hitPoint);
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