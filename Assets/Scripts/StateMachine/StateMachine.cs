using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private State[] _states;

    private State _defaultState;
    private State _currentState;
    private Coroutine _jobUpdateState;

    public event Action<IReadOnlyState> ChangedState;

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
        if (_states.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(_states));

        InitializeStates();
        SetCurrentState(_defaultState);
        RunUpdateState();
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

    private IEnumerator UpdateState()
    {
        while (true) 
        {
            _currentState.Handle();
            yield return _currentState.WaitHandle;
        }
    }

    private void InitializeStates()
    {
        foreach (State state in _states)
            state.Initialize(_character);

        _defaultState = _states[0];
    }

    private void SetCurrentState(State startState)
    {
        _currentState = startState;
        _currentState.GetedNextState += Transit;
        _currentState.Enter();
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
    }
}