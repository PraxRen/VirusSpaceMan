using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileConfig", menuName = "Combat/ProjectileConfig")]
public class ProjectileConfig : ScriptableObject
{
    [SerializeField] private Projectile _prefab;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;

    public Projectile Prefab => _prefab;
    public float Speed => _speed;
    public float Damage => _damage;
}