using UnityEngine;

[CreateAssetMenu(fileName = "NewAttack", menuName = "Combat/Attack")]
public class Attack : ScriptableObject
{
    [SerializeField] private float _damage;
    [SerializeField] private int _animationIndex;

    public float Damage => _damage;
    public int AnimationIndex => _animationIndex;
}
