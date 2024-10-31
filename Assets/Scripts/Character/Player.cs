using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
public class Player : Character
{
    private PlayerInputReader _inputReader;

    private void Update()
    {
        Mover.Move(_inputReader.DirectionMove);
    }

    protected override void AwakeAddon()
    {
        _inputReader = GetComponent<PlayerInputReader>();
    }
}