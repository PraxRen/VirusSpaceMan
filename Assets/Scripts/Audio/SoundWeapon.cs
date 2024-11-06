using UnityEngine;

public class SoundWeapon : MonoBehaviour
{
    private const float VolumeStartedAttackFactor = 0.5f;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField][SerializeInterface(typeof(IWeaponReadOnly))] private MonoBehaviour _weaponMonoBehaviour;
    [SerializeField] private AudioClip[] _clipsStartedAttack;
    
    private IWeaponReadOnly _weapon;

    private void Awake()
    {
        _weapon = (IWeaponReadOnly)_weaponMonoBehaviour;
    }

    private void OnEnable()
    {
        _weapon.StartedAttack += OnStartedAttack;
    }

    private void OnDisable()
    {
        _weapon.StartedAttack -= OnStartedAttack;
    }

    private void OnStartedAttack()
    {
        if (_clipsStartedAttack == null || _clipsStartedAttack.Length == 0)
            return;

        var index = Random.Range(0, _clipsStartedAttack.Length);
        _audioSource.PlayOneShot(_clipsStartedAttack[index], VolumeStartedAttackFactor);
    }
}