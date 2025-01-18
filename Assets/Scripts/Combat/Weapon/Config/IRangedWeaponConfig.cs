public interface IRangedWeaponConfig : IWeaponConfig
{
    public float Accuracy { get; }
    public ProjectileConfig ProjectileConfig { get; }
}