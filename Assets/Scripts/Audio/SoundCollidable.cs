using System;
using UnityEngine;

public class SoundCollidable : MonoBehaviour, ICollidable
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Collider _collider;
    [SerializeField][SerializeInterface(typeof(ISurface))] private MonoBehaviour _surfaceMonoBehaviour;
    [SerializeField] private SoundSurfaceSetting[] _soundSurfaceSettings;

    private ISurface _surface;

    public Collider Collider => _collider;

    private void OnValidate()
    {
        if (_soundSurfaceSettings != null)
            return;

        SurfaceType[] surfaceTypes = (SurfaceType[])Enum.GetValues(typeof(SurfaceType));

        if (surfaceTypes == null || surfaceTypes.Length == 0)
            return;

        _soundSurfaceSettings = new SoundSurfaceSetting[surfaceTypes.Length];

        for (int i = 0; i < _soundSurfaceSettings.Length; i++)
        {
            _soundSurfaceSettings[i] = new SoundSurfaceSetting(surfaceTypes[i]);
        }
    }

    private void OnAwake()
    {
        _surface = (ISurface)_surfaceMonoBehaviour;
    }

    public void HandleCollide(ISurface surface)
    {
        //int index = Random.Range(0, _clips.Length);
        //_audioSource.PlayOneShot(_clips[index]);
    }
}