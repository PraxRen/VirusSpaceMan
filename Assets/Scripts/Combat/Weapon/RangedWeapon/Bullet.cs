using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] private TrailRenderer _trailRenderer;

    protected override void EnableAddon()
    {
        _trailRenderer.Clear();
    }

    protected override void AfterHandleCollideAddon() => Destroy();
}
