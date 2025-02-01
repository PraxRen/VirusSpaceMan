public interface IDisplayerSlotFactory
{
    IDisplayerSlot Create(ISimpleSlot slot, IReadOnlyDisplayerStorage displayerStorage);
}