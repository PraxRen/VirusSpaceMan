using System;

public interface IReadOnlyPlaceInterest : ITarget
{
    event Action EnteredCharacter;

    bool IsEmpty { get; }
    bool HasCharacterInside { get; }
    IReadOnlyCharacter Character { get; }
}