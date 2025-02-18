using UnityEngine;

public interface IGraphicsSaleItem : ISaleItem, IGraphicsItem
{
    Vector3 OffsetPosition { get; }
    Vector3 StartRotation { get; }
    Vector3 Scale { get; }
}