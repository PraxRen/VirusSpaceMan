using System.Collections.Generic;

public interface IWeaponConfig : IObjectItem, ISurface
{
    public string IdWeapon { get; }
    public float Damage { get; }
    public float Force { get; }
    public float DistanceAttack { get; }
    public float CooldownAttack { get; }
    public float DistanceNoise { get; }
    public IReadOnlyList<Attack> Attacks { get; }
}