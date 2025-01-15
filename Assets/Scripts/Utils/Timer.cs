using System;

public class Timer
{
    private float _time;
    private bool _isRunning;

    public event Action<Timer> Completed;

    private static int _idLast;
    public int ID { get; private set; }

    public Timer(float time)
    {
        ID = ++_idLast;
        SetTime(time);
    }

    public void Tick(float deltaTime)
    {
        if (_isRunning == false)
            return;

        _time -= deltaTime;

        if (_time > 0)
            return;

        _time = 0;
        _isRunning = false;
        Completed?.Invoke(this);
    }

    public void SetTime(float time)
    {
        _isRunning = true;
        _time = time;
    }
}