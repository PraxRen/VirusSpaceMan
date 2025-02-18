using System;
using UnityEngine;
using UnityEngine.UI;

public class Armor : MonoBehaviour, IReadOnlyArmor, IDamageable, ISerializationCallbackReceiver
{
    [SerializeField][ReadOnly] private string _id;
    [SerializeField][SerializeInterface(typeof(IDamageable))] private MonoBehaviour _defaultDamageableMonoBehaviour;
    [SerializeField] private Graphics _graphics;

    private IArmorConfig _config;
    private IDamageable _currentDamageable;
    private bool _isActivated;

    public string Id => _id;
    public IArmorConfig Config => _config;
    public float FactorNoise => _config.FactorNoise;
    public SurfaceType SurfaceType => _config.SurfaceType;
    public Vector3 Position => _currentDamageable.Position;
    public Vector3 Center => _currentDamageable.Center;
    public Quaternion Rotation => _currentDamageable.Rotation;
    public Axis AxisUp => _currentDamageable.AxisUp;
    public Axis AxisForward => _currentDamageable.AxisForward;
    public Axis AxisRight => _currentDamageable.AxisRight;

    public event Action<Hit, float> BeforeTakeDamage;
    public event Action<Hit, float> AfterTakeDamage;

#if UNITY_EDITOR
    [ContextMenu("Reset ID")]
    private void ClearId()
    {
        _id = null;
    }
#endif

    private void Awake()
    {
        _currentDamageable = (IDamageable)_defaultDamageableMonoBehaviour;
    }

    public void Activate()
    {
        if (_isActivated)
            return;

        _graphics.Activate();
        _isActivated = true;
    }

    public void Deactivate()
    {
        if (_isActivated == false)
            return;

        _graphics.Deactivate();
        _isActivated = false;
    }

    public bool CanTakeDamage() =>_currentDamageable.CanTakeDamage();

    public void TakeDamage(Hit hit, float damage)
    {
        damage *= (1 - Mathf.Clamp(Config.Value, 0, GameSetting.CombatConfig.MaxValueArmor));
        BeforeTakeDamage?.Invoke(hit, damage);
        _currentDamageable.TakeDamage(hit, damage);
        AfterTakeDamage?.Invoke(hit, damage);
    }

    public bool CanDie(Hit hit, float damage) => _currentDamageable.CanDie(hit, damage);

    public bool CanReach(Transform transform) => _currentDamageable.CanReach(transform);

    public void HandleSelection() => _currentDamageable.HandleSelection();

    public void HandleDeselection() => _currentDamageable.HandleDeselection();

    void ISerializationCallbackReceiver.OnBeforeSerialize() { }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if (string.IsNullOrWhiteSpace(_id))
            _id = Guid.NewGuid().ToString();
    }
}