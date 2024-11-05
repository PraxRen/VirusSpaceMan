using UnityEngine;

[RequireComponent(typeof(IFighterReadOnly))]
public class SoundFighter : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _clips;

    private IFighterReadOnly _firhter;

    private void Awake()
    {
        _firhter = GetComponent<IFighterReadOnly>();
    }

    private void OnEnable()
    {
        //_firhter.StepTook += OnStepTook;
    }

    private void OnDisable()
    {
        //_firhter.StepTook -= OnStepTook;
    }

    private void OnStepTook()
    {
        int index = Random.Range(0, _clips.Length);
        _audioSource.PlayOneShot(_clips[index]);
    }
}