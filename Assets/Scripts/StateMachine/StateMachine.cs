using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour, IModeMoverChanger
{
    [SerializeField] private AICharacter _character;
    [SerializeField] private StateMachineConfig _config;

    private List<State> _states;
    private State _defaultState;
    private State _currentState;
    private Coroutine _jobUpdateState;

    public event Action<IReadOnlyState> ChangedState;
    public event Action<IModeMoverProvider> AddedModeMover;
    public event Action<IModeMoverProvider> RemovedModeMover;

    public IReadOnlyState State => _currentState;

    private void OnEnable()
    {
        if (_currentState == null)
            return;

        SetCurrentState(_currentState);
        RunUpdateState();
    }

    private void OnDisable()
    {
        ExitCurrentState();
        CancelJobUpdateState();
    }

    private void Start()
    {
        InitializeStates();
        SetCurrentState(_defaultState);
        RunUpdateState();
    }

    private void Update()
    {
        if (_currentState.CanUpdate())
            _currentState.Tick(Time.deltaTime);
    }

    private IEnumerator UpdateState()
    {
        while (true)
        {
            if (_currentState.CanUpdate())
                _currentState.Update();

            yield return _currentState.WaitHandle;
        }
    }

    private void RunUpdateState()
    {
        CancelJobUpdateState();
        _jobUpdateState = StartCoroutine(UpdateState());
    }

    private void CancelJobUpdateState()
    {
        if (_jobUpdateState == null)
            return;
        
        StopCoroutine(_jobUpdateState);
        _jobUpdateState = null;
    }

    private void InitializeStates()
    {
        _states = new List<State>(_config.CreatStates(_character));
        _defaultState = _states[0];
    }

    private void SetCurrentState(State startState)
    {
#if UNITY_EDITOR
        //Debug.Log($"SetCurrentState: {_character.Transform.parent.name} | {_currentState?.GetType().Name} -> {startState.GetType().Name}");
#endif
        _currentState = startState;
        _currentState.GetedNextState += Transit;
        _currentState.Enter();

        if (_currentState is IModeMoverProvider modeMoverProvider)
            AddedModeMover?.Invoke(modeMoverProvider);

        ChangedState?.Invoke(_currentState);
    }

    private void Transit(IReadOnlyState nextState)
    {
        State state = _states.FirstOrDefault(state => state.Id == nextState.Id);

        if (state == null)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"State Machine not find \"{nameof(nextState)}\"");
#endif
            return;
        }

        ExitCurrentState();
        SetCurrentState(state);
    }

    private void ExitCurrentState()
    {
        _currentState.GetedNextState -= Transit;
        _currentState.Exit();

        if (_currentState is IModeMoverProvider modeMoverProvider)
            RemovedModeMover?.Invoke(modeMoverProvider);
    }
}