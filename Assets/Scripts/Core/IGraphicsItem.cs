using UnityEngine;

public interface IGraphicsItem : IObjectItem
{
    Graphics PrefabGraphics { get; }
}