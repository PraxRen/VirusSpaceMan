using System;

public interface IAttackNotifier
{
    event Action StartingAttack;
    event Action RunningDamage;
    event Action StoppingAttack;

    bool CanCreateAttack();
    void CreateAttack();
    void CancelAttack();
}