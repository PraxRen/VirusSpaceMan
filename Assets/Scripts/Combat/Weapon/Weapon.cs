using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeaponReadOnly
{
    private WeaponConfig _config;
    private IFighterReadOnly _fighter;
    private float _lastTimeAttack = float.MaxValue;

    public WeaponConfig Config => _config;

    public bool CanAttack()
    {
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

    public void ClearConfig()
    {
        _config = null;
        _fighter = null;
    }

    protected virtual bool CanAttackAddon() => true;

    protected virtual void StartAttackAddon() { }

    protected virtual void RunDamageAddon() { }

    protected virtual void StopAttackAddon() { }
}