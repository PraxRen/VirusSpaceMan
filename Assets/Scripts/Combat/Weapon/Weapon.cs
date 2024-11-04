using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeaponReadOnly, ISerializationCallbackReceiver
{
    [SerializeField][ReadOnly] private string _id;

    private WeaponConfig _config;
    private IFighterReadOnly _fighter;
    private float _lastTimeAttack;
    private bool _isActivated;

    public event Action<Transform> Hited;

    public string Id => _id;
    public WeaponConfig Config => _config;
    public IFighterReadOnly Fighter => _fighter;

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

        StartAttackAddon();
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

    public void Init(WeaponConfig config, IFighterReadOnly fighter)
    {
        _config = config;
        _fighter = fighter;
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
        Deactivate();
        _config = null;
        _fighter = null;
    }

    protected virtual bool CanAttackAddon() => true;

    protected virtual void StartAttackAddon() { }

    protected virtual void RunDamageAddon() { }

    protected virtual void StopAttackAddon() { }

    protected void Hit(Transform transform)
    {
        Hited?.Invoke(transform);
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (string.IsNullOrWhiteSpace(_id))
            _id = System.Guid.NewGuid().ToString();
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize() { }
}