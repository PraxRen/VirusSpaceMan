using UnityEngine;

public interface IReadOnlyPlaceInterest : ITarget
{
    bool IsEmpty { get; }
    bool HasCharacterInside { get; }
    IReadOnlyCharacter Character { get; }
}