using System;
using UnityEngine;

public class Graphics : MonoBehaviour
{
    [SerializeField] private TypeGraphics _type;
    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private ParticleSystem[] _particles;

    public Transform Transform { get; private set; }
    public TypeGraphics Type => _type;

    private void Awake()
    {
        Transform = transform;
    }

    public void Activate()
    {
        foreach (var renderer in _renderers)
            renderer.enabled = true;

        foreach (var particle in _particles)
        {
            particle.Stop();
            particle.Play();
        }
    }

    public void Deactivate() 
    {
        foreach (var renderer in _renderers)
            renderer.enabled = false;

        foreach (var particle in _particles)
            particle.Stop();
    }
}