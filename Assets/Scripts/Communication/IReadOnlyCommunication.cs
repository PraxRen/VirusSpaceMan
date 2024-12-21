using System;

public interface IReadOnlyCommunication
{
    event Action<TypeCommunication> Started;
}