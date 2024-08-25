using System;

public class Wallet
{
    private readonly EnemyVisiter _visiter = new();

    public event Action OnUpdate;

    public int Score => _visiter.AccamulatedScore;

    public void onKill(Enemy enemy)
    { 
        _visiter.Visit((dynamic)enemy);
        enemy.isMoveming = false;
        OnUpdate?.Invoke();
    }

    private class EnemyVisiter : IEnemyVisiter
    {
        public int AccamulatedScore { get; private set; } = 0;

        public void Visit(PrivateSoldier visit)
        => AccamulatedScore += 7;

        public void Visit(NimbleSoldier visit)
        => AccamulatedScore += 12;

        public void Visit(ArmoredSoldier visit)
        => AccamulatedScore += 30;
    }
}
