using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SoundSurfaceSetting
{
    [SerializeField] private SurfaceType _surfaceType;
    [SerializeField] private List<AudioClip> _clips;

    public SoundSurfaceSetting(SurfaceType surfaceType)
    {
        _surfaceType = surfaceType;
        _clips = new List<AudioClip>();
    }

    public SurfaceType SurfaceType => _surfaceType;
    public IReadOnlyList<AudioClip> Clips => _clips;
}