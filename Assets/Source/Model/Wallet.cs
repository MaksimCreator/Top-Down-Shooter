public class Wallet
{
    public int Score => _visiter.AccamulatedScore;

    private EnemyVisiter _visiter = new();

    public void onKill(Enemy enemy)
    { 
        _visiter.Visit((dynamic)enemy);
        enemy.EnterStateIdel();
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
