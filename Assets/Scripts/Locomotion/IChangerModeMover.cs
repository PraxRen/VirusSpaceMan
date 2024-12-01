using System;

public interface IChangerModeMover
{
    event Action<ModeMover> ChangedModeMover;
}