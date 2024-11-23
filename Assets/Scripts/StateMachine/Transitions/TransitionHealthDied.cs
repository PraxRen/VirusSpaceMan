using UnityEngine;

public class TransitionHealthDied : Transition
{
    [SerializeField][SerializeInterface(typeof(IHealth))] private MonoBehaviour _healthMonoBehaviour;

    private IHealth _health;

    protected override void InitializeAddon()
    {
        _health = (IHealth)_healthMonoBehaviour;
    }

    protected override void ActivateAddon()
    {
        _health.Died += OnDied;
    }

    protected override void DeactivateAddon()
    {
        _health.Died -= OnDied;
    }

    private void OnDied()
    {
        SetNeedTransit();
    }
}
