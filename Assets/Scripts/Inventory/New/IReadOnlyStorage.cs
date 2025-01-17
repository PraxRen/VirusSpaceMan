using System.Collections.Generic;

internal interface IReadOnlyStorage<T> where T : Item
{
    public IReadOnlyCollection<IReadOnlySlot<T>> Slots { get; }
}