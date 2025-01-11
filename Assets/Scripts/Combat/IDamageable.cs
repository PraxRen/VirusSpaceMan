using System;
using UnityEngine;

public interface IDamageable : ITarget, ISurface, ISimpleEventInitiator
{
    event Action<Hit, float> BeforeTakeDamage;
    event Action<Hit, float> AfterTakeDamage;

    bool CanTakeDamage();
    void TakeDamage(Hit hit, float damage);
    bool CanDie(Hit hit, float damage);
}