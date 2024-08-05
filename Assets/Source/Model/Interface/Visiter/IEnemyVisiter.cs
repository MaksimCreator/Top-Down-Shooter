public interface IEnemyVisiter
{
    void Visit(PrivateSoldier visit);
    void Visit(NimbleSoldier visit);
    void Visit(ArmoredSoldier visit);
}