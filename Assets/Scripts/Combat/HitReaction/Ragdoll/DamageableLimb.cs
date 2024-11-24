using UnityEngine;

public class DamageableLimb : DamageableProvider
{
    private const string DefaultNameLayerMask = "Default";
    [SerializeField][SerializeInterface(typeof(IHealth))] private MonoBehaviour _healthMonoBehaviour;

    private IHealth _health;

    private void OnEnable()
    {
        _health.Died += OnDied;
    }

    private void OnDisable()
    {
        _health.Died -= OnDied;        
    }

    protected override void AwakeAddon()
    {
        _health = (IHealth)_healthMonoBehaviour;
    }

    private void OnDied()
    {
        gameObject.layer = LayerMask.NameToLayer(DefaultNameLayerMask);
    }
}