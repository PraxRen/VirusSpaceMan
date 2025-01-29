using System;

public interface IObjectItem : IEquatable<IObjectItem>
{
    public string Id { get; }
    public string Name { get; }
    public int Limit { get; }
    public string Description { get; }
}