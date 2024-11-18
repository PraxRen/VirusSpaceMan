using UnityEngine;

[RequireComponent(typeof(IMoverReadOnly))]
public class SoundMover : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _clips;

    private IMoverReadOnly _mover;

    private void Awake()
    {
        _mover = GetComponent<IMoverReadOnly>();
    }

    private void OnEnable()
    {
        _mover.StepTook += OnStepTook;
    }

    private void OnDisable()
    {
        _mover.StepTook -= OnStepTook;
    }

    private void OnStepTook()
    {
        int index = Random.Range(0, _clips.Length);
        _audioSource.PlayOneShot(_clips[index]);
    }
}