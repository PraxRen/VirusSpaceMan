using UnityEngine;

[CreateAssetMenu(fileName = "NewModeMover", menuName = "Locomotion/ModeMover")]
public class ModeMover : ScriptableObject
{
    [Range(-1f, 1f)][SerializeField] private float _speedMove;
    [Range(-1f, 1f)][SerializeField] private float _speedRotation;

    public float SpeedMove => _speedMove;
    public float SpeedRotation => _speedRotation;
}