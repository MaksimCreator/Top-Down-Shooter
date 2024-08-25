using UniRx;

public abstract class EntitySpawnTimer
{
    protected void StopTimer(ref (CompositeDisposable,float,float)[] CompositDisposableByTimeByDivisor)
    {
        CompositeDisposable[] allTimer = new CompositeDisposable[CompositDisposableByTimeByDivisor.Length];

        for (int i = 0; i < CompositDisposableByTimeByDivisor.Length; i++)
        {
            allTimer[i] = CompositDisposableByTimeByDivisor[i].Item1;
            CompositDisposableByTimeByDivisor[i].Item2 = Timers.GetTime(allTimer[i]) % CompositDisposableByTimeByDivisor[i].Item3;
        }

        Timers.StopRange(allTimer);
    }
}