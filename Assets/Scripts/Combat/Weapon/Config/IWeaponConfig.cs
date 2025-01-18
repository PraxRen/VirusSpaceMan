using System.Collections.Generic;

public interface IWeaponConfig : IObjectItem
{
    public string IdWeapon { get; }
    public float Damage { get; }
    public float Force { get; }
    public float DistanceAttack { get; }
    public float CooldownAttack { get; }
    public float DistanceNoise { get; }
    public float FactorAuidioVolume { get; }
    public SurfaceType SurfaceType { get; }
    public IReadOnlyList<Attack> Attacks { get; }
}