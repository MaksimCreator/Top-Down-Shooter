namespace Model
{
    public interface IPlayerDate : IService
    {
        int Score { get; }
        void TrySave(int score);
    }
}
