public class EnemySimulation : Simulated<Enemy>
{
    private readonly SpawnerEnemy _spawner;

    public EnemySimulation(EnemyViewFactory factory, Wallet walletPlayer, float cooldownSpawnEnemy)
    {
        _spawner = new(this, factory, walletPlayer, cooldownSpawnEnemy);
    }

    public void Simulated()
    {

    }

    public override void Update(float delta)
    {
        throw new System.NotImplementedException();
    }

    public void Enable()
    => _spawner.Enable();

    public void Disable()
    => _spawner.Disable();
}
