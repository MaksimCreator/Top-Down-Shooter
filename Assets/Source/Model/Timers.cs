using UniRx;
using System;
using System.Collections.Generic;

public static class Timers
{
    private static readonly Dictionary<CompositeDisposable,float> _timers = new Dictionary<CompositeDisposable,float>();

    public static CompositeDisposable StartTimer(float cooldown, Action onEnd)
    {
        CompositeDisposable disposables = new();

        Observable.Timer(TimeSpan.FromSeconds(cooldown))
            .Subscribe(_ =>
            {
                onEnd?.Invoke();
                disposables.Dispose();
            }).AddTo(disposables);

        return disposables;
    }

    public static CompositeDisposable StartInfiniteTimer(float cooldown, Action onEnd)
    {
        CompositeDisposable disposable = new();

        Observable.Interval(TimeSpan.FromSeconds(cooldown))
            .Subscribe(_ => onEnd?.Invoke())
            .AddTo(disposable);

        return disposable;
    }

    public static CompositeDisposable StartTimer(float StartValueInTimer = 0,float time = 0.05f)
    {
        CompositeDisposable disposable = new();

        Observable.Interval(TimeSpan.FromSeconds(time))
            .Subscribe(_ =>
            {
                if (_timers.ContainsKey(disposable) == false)
                    _timers.Add(disposable, StartValueInTimer);

                _timers[disposable] = _timers[disposable] + 0.05f;
            }).AddTo(disposable);

        return disposable;
    }

    public static float GetTime(CompositeDisposable timer)
    {
        float time = _timers[timer];
        _timers[timer] = 0; 
        return time;
    }

    public static void StopRange(CompositeDisposable[] timers)
    {
        foreach (CompositeDisposable disposable in timers)
        {
            disposable.Clear();
            _timers.Remove(disposable);
        }
    }
}