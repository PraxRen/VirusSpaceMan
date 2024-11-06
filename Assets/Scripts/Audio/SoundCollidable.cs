using System.Collections.Generic;
using UnityEngine;

public class SoundCollidable : MonoBehaviour, ICollidable
{
    [SerializeField] private SoundSurfaceConfig _config;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Collider _collider;
    [SerializeField][SerializeInterface(typeof(ISurface))] private MonoBehaviour _surfaceMonoBehaviour;

    private ISurface _surface;

    public Collider Collider => _collider;

    private void Awake()
    {
        _surface = (ISurface)_surfaceMonoBehaviour;
    }

    public void HandleCollide(ISurface surface)
    {
        if (_config.TryGetClips(_surface.SurfaceType, surface.SurfaceType, out IReadOnlyList<AudioClip> clips) == false)
            return;

        int index = Random.Range(0, clips.Count);
        _audioSource.PlayOneShot(clips[index]);
    }
}