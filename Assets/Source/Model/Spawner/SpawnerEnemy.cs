public class SpawnerEnemy
{

    private class EnemyVisiter : IEnemyVisiter
    {
        private readonly PoolObject<Enemy> _privateSoldier;
        private readonly PoolObject<Enemy> _nimbleSoldier;
        private readonly PoolObject<Enemy> _armorSoldier;

        public PoolObject<Enemy> PoolObject { get; private set; }

        public void Visit(PrivateSoldier visit)
        => PoolObject = _privateSoldier;

        public void Visit(NimbleSoldier visit)
        => PoolObject = _nimbleSoldier;

        public void Visit(ArmoredSoldier visit)
        => PoolObject = _armorSoldier;
    }
}
