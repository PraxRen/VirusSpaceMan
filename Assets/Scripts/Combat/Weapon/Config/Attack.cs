using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "Combat/Attack")]
public class Attack : ScriptableObject
{
    [SerializeField] private float _damage;
    [SerializeField] private int _animationIndex;
    [Range(0f, 1f)][SerializeField] private float _ragePoints;

    public float Damage => _damage;
    public int AnimationIndex => _animationIndex;
    public float RagePoints => _ragePoints;
}
