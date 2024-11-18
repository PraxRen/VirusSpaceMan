using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SoundSurfaceSetting
{
    [SerializeField] private SurfaceType _surfaceTypeOne;
    [SerializeField] private SurfaceType _surfaceTypeTwo;
    [SerializeField] private List<AudioClip> _clips;

    public SoundSurfaceSetting()
    {
        _surfaceTypeOne = SurfaceType.None;
        _surfaceTypeTwo = SurfaceType.None;
        _clips = new List<AudioClip>();
    }

    public SurfaceType SurfaceTypeOne => _surfaceTypeOne;
    public SurfaceType SurfaceTypeTwo => _surfaceTypeTwo;
    public IReadOnlyList<AudioClip> Clips => _clips;

    public override bool Equals(object obj)
    {

        if (SurfaceTypeOne == SurfaceType.None || SurfaceTypeTwo == SurfaceType.None)
            return false;

        if (obj is SoundSurfaceSetting other)
        {
            if (other.SurfaceTypeOne == SurfaceType.None || other.SurfaceTypeTwo == SurfaceType.None)
                return false;

            return (SurfaceTypeOne == other.SurfaceTypeOne && SurfaceTypeTwo == other.SurfaceTypeTwo) ||
                   (SurfaceTypeOne == other.SurfaceTypeTwo && SurfaceTypeTwo == other.SurfaceTypeOne);
        }

        return false;
    }

    public override int GetHashCode()
    {
        int hash1 = SurfaceTypeOne.GetHashCode() ^ SurfaceTypeTwo.GetHashCode();
        int hash2 = SurfaceTypeTwo.GetHashCode() ^ SurfaceTypeOne.GetHashCode();

        return hash1 ^ hash2;
    }
}