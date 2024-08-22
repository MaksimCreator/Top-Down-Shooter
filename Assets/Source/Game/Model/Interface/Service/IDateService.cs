public interface IDateService : IService
{
    int Score { get; }
    void TrySave(int score);
}