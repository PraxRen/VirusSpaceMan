using System.Linq;
using UnityEngine;

public class HandlerHit : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IDamageable))] private MonoBehaviour _damageableMonoBehaviour;
    [SerializeField][SerializeInterface(typeof(IHitReaction))] private MonoBehaviour[] _hitReactionsMonoBehaviour;

    private IDamageable _damageable;
    private IHitReaction[] _reactions;

    private void Awake()
    {
        _damageable = (IDamageable)_damageableMonoBehaviour;
        _reactions = _hitReactionsMonoBehaviour.Cast<IHitReaction>().ToArray();
    }

    private void OnEnable()
    {
        _damageable.BeforeTakeDamage += OnBeforeTakeDamage;
    }

    private void OnDisable()
    {
        _damageable.BeforeTakeDamage -= OnBeforeTakeDamage;
    }

    private void OnBeforeTakeDamage(Hit hit, float damage)
    {
        foreach (IHitReaction reaction in _reactions)
        {
            if (reaction.CanHandleHit(hit, damage))
                reaction.HandleHit(hit, damage);
        }
    }
}