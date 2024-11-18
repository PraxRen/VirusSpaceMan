using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileConfig", menuName = "Combat/ProjectileConfig")]
public class ProjectileConfig : ScriptableObject
{
    [SerializeField] private Projectile _prefab;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [Range(0f,1f)][SerializeField] private float _factorNoise;
    [SerializeField] private SurfaceType _surfaceType;

    public Projectile Prefab => _prefab;
    public float Speed => _speed;
    public float Damage => _damage;
    public float FactorNoise => _factorNoise;
    public SurfaceType SurfaceType => _surfaceType;
}