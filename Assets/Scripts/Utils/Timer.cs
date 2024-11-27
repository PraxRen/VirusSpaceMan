using System;

public class Timer
{
    private float _time;
    private bool _isRunning;

    public event Action<Timer> Ñompleted;

    public Timer(float time)
    {
        _time = time;
        _isRunning = true;
    }

    public void Tick(float deltaTime)
    {
        if (_isRunning == false)
            return;

        _time -= deltaTime;

        if (_time <= 0)
        {
            _time = 0;
            _isRunning = false;
            Ñompleted?.Invoke(this);
        }
    }
}