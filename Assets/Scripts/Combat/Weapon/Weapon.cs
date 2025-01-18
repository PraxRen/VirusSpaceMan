using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public abstract class Weapon : MonoBehaviour, IWeaponReadOnly, ISerializationCallbackReceiver
{
    private float TimeResetIndexAttack = 2f;

    [SerializeField][ReadOnly] private string _id;
    [SerializeField] private GameObject _graphics;
    [SerializeField] private Collider[] _colliders;

    private IWeaponConfig _config;
    private IFighterReadOnly _fighter;
    private float _lastTimeAttack;
    private bool _isActivated;
    private int _indexAttack;
    private Transform _transform;

    public event Action StartedAttack;
    public event Action<IDamageable, IWeaponReadOnly, Attack, Vector3> Hited;

    public string Id => _id;
    public IWeaponConfig Config => _config;
    public IFighterReadOnly Fighter => _fighter;
    public Attack CurrentAttack => _config.Attacks[_indexAttack];
    public float FactorNoise => _config.FactorAuidioVolume;
    public SurfaceType SurfaceType => _config.SurfaceType;
    public Vector3 Position => _transform.position;
    public IReadOnlyCollection<Collider> Colliders => _colliders;
    protected Transform Transform => _transform;

#if UNITY_EDITOR
    [ContextMenu("Reset ID")]
    private void ClearId()
    {
        _id = null;
    }
#endif
    private void Awake()
    {
        _transform = transform;
        AwakeAddon();
    }

    private void OnEnable()
    {
        Activate();
        EnableAddon();
    }

    private void OnDisable()
    {
        Deactivate();
        DisableAddon();
    }

    private void Start()
    {
        Deactivate();
        StartAddon();
    }

    public float GetDamage()
    {
        return _config.Damage + GetDamageAddon();
    }

    public void UpdateIndexAttack()
    {
        if (Time.time - _lastTimeAttack > TimeResetIndexAttack + _config.CooldownAttack)
        {
            ResetIndexAttack();
        }

        int indexNext = _indexAttack + 1;

        if (indexNext > _config.Attacks.Count - 1)
            indexNext = 0;

        _indexAttack = indexNext;
        Debug.Log($"UpdateIndexAttack: {((Fighter)Fighter).transform.parent.name} | {_indexAttack}");
    }

    public bool CanAttack()
    {
        if (_isActivated == false)
            return false;

        if (_config == null)
            return false;

        if (_fighter == null)
            return false;

        if (Time.time - _lastTimeAttack < _config.CooldownAttack)
            return false;

        return CanAttackAddon();
    }

    public void StartAttack()
    {
        if (_config == null)
            throw new ArgumentNullException(nameof(_config));

        if (_fighter == null)
            throw new ArgumentNullException(nameof(_fighter));

        _lastTimeAttack = Time.time;
        StartAttackAddon();
        StartedAttack?.Invoke();
    }

    public void RunDamage()
    {
        if (_config == null)
            throw new ArgumentNullException(nameof(_config));

        if (_fighter == null)
            throw new ArgumentNullException(nameof(_fighter));


        RunDamageAddon();
    }

    public void StopAttack()
    {
        if (_config == null)
            throw new ArgumentNullException(nameof(_config));

        if (_fighter == null)
            throw new ArgumentNullException(nameof(_fighter));

        StopAttackAddon();
    }

    public void Initialize(IWeaponConfig config, IFighterReadOnly fighter)
    {
        _config = config;
        _fighter = fighter;
        InitializeAddon(config, fighter);
    }

    public void Activate()
    {
        if (_isActivated)
            return;

        _graphics.SetActive(true);
        ResetIndexAttack();
        ActivateAddon();
        _isActivated = true;
    }

    public void Deactivate() 
    {
        if (_isActivated == false)
            return;

        _graphics.SetActive(false);
        DeactivateAddon();
        _isActivated = false;
    }

    public void ClearConfig()
    {
        _config = null;
        _fighter = null;
    }

    public bool CanCollide(Collider targetCollider)
    {
        if (SimpleUtils.IsLayerInclud(targetCollider.gameObject, _fighter.LayerMaskCollision) == false)
            return false;

        if (_fighter.IgnoreColliders.Any(collider => collider == targetCollider))
            return false;

        return true;
    }

    protected void HandleCollide(Collider targetCollider, Attack attack, Vector3 hitPoint)
    {
        if (targetCollider.TryGetComponent(out ICollidable collidable))
        {
            HandleCollidable(collidable);
        }

        if (targetCollider.TryGetComponent(out IDamageable damageable))
        {
            if (SimpleUtils.IsLayerInclud(targetCollider.gameObject, _fighter.LayerMaskDamageable))
            {
                Hited?.Invoke(damageable, this, attack, hitPoint);
            }
        }
    }

    protected virtual void HandleCollidable(ICollidable collidable) 
    {
        collidable.HandleCollide(this);
    } 

    protected virtual float GetDamageAddon() => 0f;

    protected virtual void AwakeAddon() { }
    
    protected virtual void StartAddon() { }
    
    protected virtual void EnableAddon() { }
    
    protected virtual void DisableAddon() { }

    protected virtual void InitializeAddon(IWeaponConfig config, IFighterReadOnly fighter) { }

    protected virtual void ActivateAddon() { }

    protected virtual void DeactivateAddon() { } 

    protected virtual bool CanAttackAddon() => true;

    protected virtual void StartAttackAddon() { }

    protected virtual void RunDamageAddon() { }

    protected virtual void StopAttackAddon() { }

    private void ResetIndexAttack()
    {
        _indexAttack = -1;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (string.IsNullOrWhiteSpace(_id))
            _id = Guid.NewGuid().ToString();
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize() { }
}