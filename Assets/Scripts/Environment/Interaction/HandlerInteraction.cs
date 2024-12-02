using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(ActionScheduler))]
public class HandlerInteraction : MonoBehaviour, IAction
{
    [SerializeField][SerializeInterface(typeof(IInteractionNotifier))] private MonoBehaviour _interactionNotifierMonoBehaviour;

    private Transform _transform;
    private Mover _mover;
    private ActionScheduler _actionScheduler;
    private IInteractionNotifier _interactionNotifier;
    private Coroutine _jobMoveToObjectInteraction;
    private IObjectInteraction _currentObjectInteraction;

    private void Awake()
    {
        _transform = transform;
        _mover = GetComponent<Mover>();
        _actionScheduler = GetComponent<ActionScheduler>();
        _interactionNotifier = (IInteractionNotifier)_interactionNotifierMonoBehaviour;
    }

    private void OnEnable()
    {
        _interactionNotifier.BeforeInteract += OnBeforeInteract;
        _interactionNotifier.Interacted += OnInteracted;
        _interactionNotifier.AfterInteract += OnAfterInteract;
    }

    private void OnDisable()
    {
        _interactionNotifier.BeforeInteract -= OnBeforeInteract;
        _interactionNotifier.Interacted -= OnInteracted;
        _interactionNotifier.AfterInteract -= OnAfterInteract;
    }

    public void Cancel()
    {
        CancelMoveToObjectInteraction();
    }

    public bool CanStartInteract(IObjectInteraction objectInteraction)
    {
        return _interactionNotifier.CanRun();
    }

    public void StartInteract(IObjectInteraction objectInteraction)
    {
        _actionScheduler.StartAction(this);
        _actionScheduler.SetBlock(this);
        _currentObjectInteraction = objectInteraction;
        CancelMoveToObjectInteraction();
        _jobMoveToObjectInteraction = StartCoroutine(MoveToObjectInteraction());
    }

    private void CancelMoveToObjectInteraction()
    {
        if (_jobMoveToObjectInteraction == null)
            return;

        StopCoroutine(_jobMoveToObjectInteraction);
        _jobMoveToObjectInteraction = null;
    }

    private IEnumerator MoveToObjectInteraction()
    {
        float factorForward = -1;
        float factorForwardLimit = 0.9f;

        while (_mover.CanMove() && _currentObjectInteraction.CanReach(_transform) == false && factorForward < factorForwardLimit) 
        {
            factorForward = Vector3.Dot(_currentObjectInteraction.Rotation * Vector3.forward, _transform.forward);
            Vector3 directionVector3 = (_currentObjectInteraction.Position - _transform.position).normalized;
            Vector2 direction = new Vector2(directionVector3.x, directionVector3.z);
            _mover.Move(direction);
            _mover.LookAtDirection(direction);
            yield return null;
        }

        _transform.position = _currentObjectInteraction.Position;
        _transform.forward = _currentObjectInteraction.Rotation * Vector3.forward;
        _jobMoveToObjectInteraction = null;
        _interactionNotifier.Run();
    }

    private void OnBeforeInteract()
    {

    }

    private void OnInteracted()
    {

    }

    private void OnAfterInteract()
    {

    }
}