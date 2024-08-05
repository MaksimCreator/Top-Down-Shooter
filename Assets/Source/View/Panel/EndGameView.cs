using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameView : MonoBehaviour
{
    [SerializeField] private Image _imageNewBest;
    [SerializeField] private Button _restaet;
    [SerializeField] private Button _menu;

    public void Enter(int score)
    {
        Time.timeScale = 0;

        if (PlayerPrefs.GetInt(Constant.MyBest, 0) < score)
        {
            PlayerPrefs.SetInt(Constant.MyBest,score);
            _imageNewBest.gameObject.SetActive(true);
        }

        _restaet.onClick.RemoveAllListeners();
        _menu.onClick.RemoveAllListeners();

        _menu.onClick.AddListener(() => LoadScene(SceneManager.GetActiveScene().buildIndex + 1));

        _restaet.onClick.AddListener(() => LoadScene(SceneManager.GetActiveScene().buildIndex));

        void LoadScene(int index)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(index);
        }
    } 
}