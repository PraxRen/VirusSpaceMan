using System;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField][SerializeInterface(typeof(IReadOnlyCharacter))] private MonoBehaviour _characterMonoBehaviour;
    [SerializeField] private State[] _states;

    private IReadOnlyCharacter _character;
    private State _defaultState;
    private State _currentState;

    public event Action<IReadOnlyState> ChangedState;

    public IReadOnlyState State => _currentState;

    private void Awake()
    {
        _character = (IReadOnlyCharacter)_characterMonoBehaviour;
    }

    private void OnEnable()
    {
        SetCurrentState(_currentState);
    }

    private void OnDisable()
    {
        ExitCurrentState();
    }

    private void Start()
    {
        InitializeStates();
        SetCurrentState(_defaultState);
    }

    private void Update()
    {
        if (_currentState == null)
            return;

        float deltasTime = Time.deltaTime;

        if (_currentState.CanHandle(deltasTime))
            return;

        _currentState.Handle(deltasTime);
    }

    private void InitializeStates()
    {
        if (_states.Length == 0)
            return;

        foreach (State state in _states)
            state.Initialize(_character);

        _defaultState = _states[0];
    }

    private void SetCurrentState(State startState)
    {
        if (startState == null)
            return;

        _currentState = startState;
        _currentState.GetedNextState += Transit;
        _currentState.Enter();
        ChangedState?.Invoke(_currentState);
    }

    private void Transit(IReadOnlyState nextState)
    {
        State state = _states.FirstOrDefault(state => state.Id == nextState.Id);

        if (state == null)
            return;

        ExitCurrentState();
        SetCurrentState(state);
    }

    private void ExitCurrentState()
    {
        if (_currentState == null)
            return;

        _currentState.GetedNextState -= Transit;
        _currentState.Exit();
    }
}