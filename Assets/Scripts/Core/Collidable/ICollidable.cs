using UnityEngine;

public interface ICollidable
{
    Collider Collider { get; }

    void HandleCollide(ISurface surface);
}