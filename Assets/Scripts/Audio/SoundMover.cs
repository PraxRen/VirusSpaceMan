using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IMoverReadOnly))]
public class SoundMover : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourceStep;
    [SerializeField] private AudioClip[] _stepClips;

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
        int index = UnityEngine.Random.Range(0, _stepClips.Length);
        _audioSourceStep.PlayOneShot(_stepClips[index]);
    }
}