using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "NewSoundSurfaceConfig", menuName = "Audio/SoundSurfaceConfig")]
public class SoundSurfaceConfig : ScriptableObject
{
    [SerializeField] private SoundSurfaceSetting[] _settings;

#if UNITY_EDITOR
    private int _lastCountSettings;


    private void OnValidate()
    {
        if (_settings == null)
            return;

        if (_settings.Length > _lastCountSettings)
        {
            _settings[_settings.Length - 1] = new SoundSurfaceSetting();
        }

        SoundSurfaceSetting[] newSettings = _settings.Distinct().ToArray();

        if (newSettings.Length != _settings.Length)
        {
            Debug.LogWarning($"Cannot add duplicate value {nameof(SoundSurfaceSetting)}");
        }

        _settings = newSettings;
        _lastCountSettings = _settings.Length;
    }
#endif

    public bool TryGetClips(SurfaceType surfaceTypeOne, SurfaceType surfaceTypeTwo, out IReadOnlyList<AudioClip> clips)
    {
        clips = null;

        if (surfaceTypeOne == SurfaceType.None || surfaceTypeTwo == SurfaceType.None)
            return false;

        SoundSurfaceSetting soundSurfaceSetting = FindSoundSurfaceSetting(surfaceTypeOne, surfaceTypeTwo);

        if(soundSurfaceSetting == null)
            return false;

        clips = soundSurfaceSetting.Clips;

        return clips != null;       
    }

    private SoundSurfaceSetting FindSoundSurfaceSetting(SurfaceType surfaceTypeOne, SurfaceType surfaceTypeTwo)
    {
        if (surfaceTypeOne == SurfaceType.None || surfaceTypeTwo == SurfaceType.None)
            throw new InvalidOperationException($"No value selected for {nameof(surfaceTypeOne)} or {nameof(surfaceTypeTwo)}");
    
        if (surfaceTypeOne > surfaceTypeTwo)
        {
            SurfaceType firstSurFaceType = surfaceTypeTwo;
            surfaceTypeTwo = surfaceTypeOne;
            surfaceTypeOne = firstSurFaceType;
        }

        SoundSurfaceSetting soundSurfaceSetting = _settings.FirstOrDefault(setting => setting.SurfaceTypeOne == surfaceTypeOne && setting.SurfaceTypeTwo == surfaceTypeTwo);
        return soundSurfaceSetting;
    }
}