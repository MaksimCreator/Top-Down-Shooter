using System;
using UniRx;

public class Timer
{
    public static void StartTimer(float cooldown, Action onEnd)
    {
        CompositeDisposable disposables = new();

        Observable.Timer(TimeSpan.FromSeconds(cooldown))
            .Subscribe(_ =>
            {
                onEnd?.Invoke();
                disposables.Dispose();
            }).AddTo(disposables);
    }

    public static IDisposable StartInfiniteTimer(float cooldown, Action onEnd)
    {
        CompositeDisposable disposable = new();

        Observable.Interval(TimeSpan.FromSeconds(cooldown))
            .Subscribe(_ => onEnd?.Invoke())
            .AddTo(disposable);

        return disposable;
    }
}