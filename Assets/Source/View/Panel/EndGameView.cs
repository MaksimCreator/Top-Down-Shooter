using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _imageNewBest;
    [SerializeField] private Button _restaet;
    [SerializeField] private Button _menu;

    public void Enter(int score)
    {
        if (PlayerPrefs.GetInt(Constant.MyBest, 0) < score)
        {
            PlayerPrefs.SetInt(Constant.MyBest,score);
            _imageNewBest.gameObject.SetActive(true);
        }

        _restaet.onClick.RemoveAllListeners();
        _menu.onClick.RemoveAllListeners();

        _menu.onClick.AddListener(() => SceneManager.LoadScene(Constant.StartSceneIndex));
        _restaet.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));

        gameObject.SetActive(true);
    } 
}