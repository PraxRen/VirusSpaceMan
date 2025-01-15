using UnityEngine;

public interface IReadOnlyObjectInteraction
{
    ObjectInteractionConfig Config { get; }
    ITarget StartPoint { get; }
    ITarget LookAtPoint { get; }
    int IdIteration { get; }
    int IndexIteration { get; }
    int Layer {  get; }
}