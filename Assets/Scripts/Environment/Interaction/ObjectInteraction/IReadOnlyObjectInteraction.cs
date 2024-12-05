public interface IReadOnlyObjectInteraction
{
    ObjectInteractionConfig Config { get; }
    ITarget StartPoint { get; }
    ITarget LookAtPoint { get; }
    int AnimationInteractiveIndex { get; }
}