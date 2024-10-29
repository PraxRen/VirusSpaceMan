using System;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(IMoverReadOnly))]
public class AnimatorMover : MonoBehaviour, ICreatorStep
{
    [Range(0, 1)][SerializeField] private float _weightForAnimationEvent;

    private Animator _animator;
    private IMoverReadOnly _mover;
    
    public event Action CreatedStep;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<IMover>();
    }

    private void Update()
    {
        _animator.SetFloat(CharacterAnimatorData.Params.Speed, _mover.Speed);
    }

    //AnimationEvent
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;

        CreatedStep?.Invoke();
    }
}