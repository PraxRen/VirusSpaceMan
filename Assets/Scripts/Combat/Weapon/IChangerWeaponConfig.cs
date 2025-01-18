using System;

public interface IChangerWeaponConfig 
{
    event Action<IWeaponConfig> ChangedWeaponConfig;
    event Action RemovedWeaponConfig;
}