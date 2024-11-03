using UnityEngine;

[CreateAssetMenu(fileName = "NewModeMover", menuName = "Locomotion/ModeMover")]
public class ModeMover : ScriptableObject
{
    [Min(0f)][SerializeField] private float _speedMove;
    [Min(0f)][SerializeField] private float _speedRotation;

    public float SpeedMove => _speedMove;
    public float SpeedRotation => _speedRotation;
}