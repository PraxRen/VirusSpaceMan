using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LayerBullet : Projectile
{
    [SerializeField] private ParticleSystem[] _particleSystemForEnable;

    protected override void EnableAddon()
    {
        foreach (ParticleSystem particleSystem in _particleSystemForEnable)
        {
            particleSystem.Stop();
            particleSystem.Play();
        }
    }

    protected override void DisableAddon()
    {
        foreach (ParticleSystem particleSystem in _particleSystemForEnable)
        {
            particleSystem.Stop();
        }
    }

    protected override void AfterHandleCollideAddon(Collision collision) => Destroy();
}