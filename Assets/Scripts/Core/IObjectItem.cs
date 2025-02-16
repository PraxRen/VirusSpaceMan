using System;

public interface IObjectItem : IEquatable<IObjectItem>
{
    public string Id { get; }
    public string Name { get; }
    public int LimitInSlot { get; }
    public int LimitInAllStorage { get; }
    public string Description { get; }
}