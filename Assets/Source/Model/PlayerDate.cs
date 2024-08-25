using UnityEngine;

namespace Model
{
    public class PlayerDate : IPlayerDate
    {
        private int MyBest = PlayerPrefs.GetInt(Constant.MyBest, 0);

        public int Score => MyBest;

        public void TrySave(int score)
        {
            if (MyBest < score)
            {
                PlayerPrefs.SetInt(Constant.MyBest, score);
                MyBest = score;
            }
        }
    }
}
