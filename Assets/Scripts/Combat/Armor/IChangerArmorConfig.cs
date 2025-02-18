using System;

public interface IChangerArmorConfig
{
    event Action<IArmorConfig> ChangedArmorConfig;
    event Action RemovedArmorConfig;
}