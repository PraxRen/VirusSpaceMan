using UnityEngine;

[RequireComponent(typeof(PlayerInputReader), typeof(Fighter), typeof(ScannerDamageable))]
public class Player : Character
{
    private PlayerInputReader _inputReader;
    private ScannerDamageable _scannerDamageable;
    private Fighter _fighter;

    protected override void AwakeAddon()
    {
        _inputReader = GetComponent<PlayerInputReader>();
        _fighter = GetComponent<Fighter>();
        _scannerDamageable = GetComponent<ScannerDamageable>();
    }

    private void Update()
    {
        Mover.Move(_inputReader.DirectionMove);
        HandleCombat();
    }

    private void HandleCombat()
    {
        if (_scannerDamageable.Target == null)
            return;

        if (_fighter.CanAttack() == false)
            return;

        _fighter.Attack(_scannerDamageable.Target);
    }
}