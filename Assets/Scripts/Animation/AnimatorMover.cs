using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorMover : MonoBehaviour, IStepNotifier
{
    [SerializeField][SerializeInterface(typeof(IMoverReadOnly))] private MonoBehaviour _moverMonoBehaviour;
    [Range(0, 1)][SerializeField] private float _weightForAnimationEvent;

    private Animator _animator;
    private IMoverReadOnly _mover;
    
    public event Action CreatedStep;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = (IMoverReadOnly)_moverMonoBehaviour;
    }

    private void Update()
    {
        Vector3 velocity = transform.InverseTransformDirection(_mover.Velocity);
        _animator.SetFloat(CharacterAnimatorData.Params.VelocityX, velocity.x);
        _animator.SetFloat(CharacterAnimatorData.Params.VelocityZ, velocity.z);
    }

    //AnimationEvent
    private void OnFootstepAnimationEvent(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight < _weightForAnimationEvent)
            return;

        CreatedStep?.Invoke();
    }
}