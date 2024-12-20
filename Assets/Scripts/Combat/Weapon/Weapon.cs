using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeaponReadOnly, ISerializationCallbackReceiver
{
    private float TimeResetIndexAttack = 2f;

    [SerializeField][ReadOnly] private string _id;
    [SerializeField] private Collider[] _colliders;

    private WeaponConfig _config;
    private IFighterReadOnly _fighter;
    private float _lastTimeAttack;
    private bool _isActivated;
    private int _indexAttack;
    private Transform _transform;

    public event Action StartedAttack;
    public event Action<IDamageable, IWeaponReadOnly, Attack, Vector3> Hited;

    public string Id => _id;
    public WeaponConfig Config => _config;
    public IFighterReadOnly Fighter => _fighter;
    public Attack CurrentAttack => _config.Attacks[_indexAttack];
    public float FactorNoise => _config.FactorNoise;
    public SurfaceType SurfaceType => _config.SurfaceType;
    public Vector3 Position => _transform.position;
    public IReadOnlyCollection<Collider> Colliders => _colliders;
    protected Transform Transform => _transform;

    private void Awake()
    {
        _transform = transform;
        AwakeAddon();
    }

    private void OnEnable()
    {
        ResetIndexAttack();
        EnableAddon();
    }

    private void OnDisable()
    {
        DisableAddon();
    }

    private void Start()
    {
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

    public void Initialize(WeaponConfig config, IFighterReadOnly fighter)
    {
        _config = config;
        _fighter = fighter;
        InitializeAddon(config, fighter);
    }

    public void Activate()
    {
        if (_isActivated)
            return;

        gameObject.SetActive(true);
        _isActivated = true;
    }

    public void Deactivate() 
    {
        if (_isActivated == false)
            return;

        gameObject.SetActive(false);
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

    protected virtual void InitializeAddon(WeaponConfig config, IFighterReadOnly fighter) { }

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