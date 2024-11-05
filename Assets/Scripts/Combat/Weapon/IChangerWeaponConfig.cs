using System;

public interface IChangerWeaponConfig 
{
    event Action<WeaponConfig> ChangedWeaponConfig;
    event Action RemovedWeaponConfig;
}